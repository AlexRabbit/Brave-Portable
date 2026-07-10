using System.Diagnostics;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BraveNightlyPortable;

internal static class Program
{
    private const string ChannelKeyword = "Nightly";
    private const string GitHubReleasesApi = "https://api.github.com/repos/brave/brave-browser/releases?per_page=100";
    private const string GitHubReleasesPage = "https://github.com/brave/brave-browser/releases";
    private const string UserAgent = "BraveNightlyPortable-Updater/1.0";

    [STAThread]
    private static int Main(string[] args)
    {
        try
        {
            var root = FindPackageRoot();
            if (root is null)
            {
                Console.Error.WriteLine("Could not locate BraveNightlyPortable package root.");
                return 1;
            }

            TryUpdate(root);

            var internalLauncher = Path.Combine(root, "BraveNightlyPortable-Internal.exe");
            if (!File.Exists(internalLauncher))
            {
                Console.Error.WriteLine($"Internal launcher missing: {internalLauncher}");
                return 1;
            }

            return Launch(root, internalLauncher, args);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            // Never block browser launch because update failed
            var root = FindPackageRoot();
            if (root is null) return 1;
            var fallback = Path.Combine(root!, "BraveNightlyPortable-Internal.exe");
            return File.Exists(fallback) ? Launch(root!, fallback, args) : 1;
        }
    }

    private static string? FindPackageRoot()
    {
        var start = Path.GetDirectoryName(Environment.ProcessPath ?? AppContext.BaseDirectory);
        if (string.IsNullOrEmpty(start)) return null;

        var dir = start.TrimEnd(Path.DirectorySeparatorChar);
        for (var i = 0; i < 8; i++)
        {
            var marker = Path.Combine(dir, "App", "AppInfo", "appinfo.ini");
            if (File.Exists(marker)) return dir;
            var parent = Directory.GetParent(dir)?.FullName;
            if (string.IsNullOrEmpty(parent) || parent == dir) break;
            dir = parent;
        }
        return null;
    }

    private static void TryUpdate(string root)
    {
        var braveDir = Path.Combine(root, "App", "Brave");
        var braveExe = Path.Combine(braveDir, "brave.exe");
        var needsInstall = !File.Exists(braveExe);

        var current = needsInstall ? null : GetBraveVersion(braveExe);
        var latest = GetLatestNightlyRelease() ?? GetLatestNightlyFromHtml();
        if (latest is null) return;

        if (!needsInstall && current is not null && CompareVersion(current, latest.Version) >= 0) return;

        DownloadAndInstall(root, braveDir, latest);
    }

    private static ReleaseInfo? GetLatestNightlyRelease()
    {
        using var client = CreateClient();
        try
        {
            using var stream = client.GetStreamAsync(GitHubReleasesApi).GetAwaiter().GetResult();
            using var doc = JsonDocument.Parse(stream);
            foreach (var release in doc.RootElement.EnumerateArray())
            {
                if (!release.TryGetProperty("name", out var nameEl)) continue;
                var name = nameEl.GetString() ?? "";
                if (!name.Contains(ChannelKeyword, StringComparison.OrdinalIgnoreCase)) continue;

                if (!release.TryGetProperty("tag_name", out var tagEl)) continue;
                var version = tagEl.GetString()?.TrimStart('v') ?? "";
                if (string.IsNullOrWhiteSpace(version)) continue;

                if (!release.TryGetProperty("assets", out var assets)) continue;
                foreach (var asset in assets.EnumerateArray())
                {
                    var assetName = asset.GetProperty("name").GetString() ?? "";
                    if (!Regex.IsMatch(assetName, @"^brave-v[\d\.]+-win32-x64\.zip$")) continue;
                    var url = asset.GetProperty("browser_download_url").GetString();
                    if (string.IsNullOrEmpty(url)) continue;
                    return new ReleaseInfo(version, assetName, url);
                }
            }
        }
        catch
        {
            return null;
        }
        return null;
    }

    private static ReleaseInfo? GetLatestNightlyFromHtml()
    {
        using var client = CreateClient();
        try
        {
            var html = client.GetStringAsync(GitHubReleasesPage).GetAwaiter().GetResult();
            // Match release title lines containing Nightly and zip asset links
            var tagMatches = Regex.Matches(html, @"Nightly v(\d+\.\d+\.\d+)", RegexOptions.IgnoreCase);
            foreach (Match tagMatch in tagMatches)
            {
                var version = tagMatch.Groups[1].Value;
                var assetName = $"brave-v{version}-win32-x64.zip";
                if (html.Contains(assetName, StringComparison.OrdinalIgnoreCase))
                {
                    var url = $"https://github.com/brave/brave-browser/releases/download/v{version}/{assetName}";
                    return new ReleaseInfo(version, assetName, url);
                }
            }
        }
        catch
        {
            return null;
        }
        return null;
    }

    private static void DownloadAndInstall(string root, string braveDir, ReleaseInfo release)
    {
        var zipPath = Path.Combine(Path.GetTempPath(), release.AssetName);
        var staging = Path.Combine(Path.GetTempPath(), "BravePortableUpdate_" + Guid.NewGuid().ToString("N"));

        try
        {
            using var client = CreateClient();
            using (var response = client.GetAsync(release.DownloadUrl, HttpCompletionOption.ResponseHeadersRead).GetAwaiter().GetResult())
            {
                response.EnsureSuccessStatusCode();
                using var fs = File.Create(zipPath);
                response.Content.CopyToAsync(fs).GetAwaiter().GetResult();
            }

            foreach (var proc in Process.GetProcessesByName("brave"))
            {
                try { proc.Kill(); } catch { /* ignore */ }
            }
            Thread.Sleep(1000);

            Directory.CreateDirectory(staging);
            ZipFile.ExtractToDirectory(zipPath, staging);

            var extractedBrave = FindBraveExe(staging);
            if (extractedBrave is null) throw new InvalidOperationException("Downloaded package missing brave.exe");

            VerifyNightlyBuild(extractedBrave);

            var extractedRoot = Path.GetDirectoryName(extractedBrave)!;
            foreach (var entry in Directory.EnumerateFileSystemEntries(braveDir))
            {
                try
                {
                    if (Directory.Exists(entry)) Directory.Delete(entry, true);
                    else File.Delete(entry);
                }
                catch { /* best effort */ }
            }

            CopyDirectory(extractedRoot, braveDir);
            UpdateAppInfo(root, release.Version);
        }
        finally
        {
            try { if (File.Exists(zipPath)) File.Delete(zipPath); } catch { }
            try { if (Directory.Exists(staging)) Directory.Delete(staging, true); } catch { }
        }
    }

    private static void VerifyNightlyBuild(string braveExe)
    {
        var info = FileVersionInfo.GetVersionInfo(braveExe);
        var product = info.ProductName ?? "";
        if (!product.Contains("Nightly", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException($"Refusing to install non-Nightly build: {product}");
    }

    private static string? FindBraveExe(string dir)
    {
        if (File.Exists(Path.Combine(dir, "brave.exe"))) return Path.Combine(dir, "brave.exe");
        foreach (var sub in Directory.EnumerateDirectories(dir))
        {
            var candidate = Path.Combine(sub, "brave.exe");
            if (File.Exists(candidate)) return candidate;
        }
        return null;
    }

    private static void CopyDirectory(string source, string dest)
    {
        Directory.CreateDirectory(dest);
        foreach (var file in Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories))
        {
            var rel = Path.GetRelativePath(source, file);
            var target = Path.Combine(dest, rel);
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            File.Copy(file, target, true);
        }
    }

    private static void UpdateAppInfo(string root, string version)
    {
        var appInfo = Path.Combine(root, "App", "AppInfo", "appinfo.ini");
        if (!File.Exists(appInfo)) return;
        var parts = version.Split('.');
        var packageVersion = parts.Length >= 3 ? $"{parts[0]}.{parts[1]}.{parts[2]}.0" : "0.0.0.0";
        var lines = File.ReadAllLines(appInfo);
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("PackageVersion=", StringComparison.Ordinal))
                lines[i] = $"PackageVersion={packageVersion}";
            else if (lines[i].StartsWith("DisplayVersion=", StringComparison.Ordinal))
                lines[i] = $"DisplayVersion={version} Nightly";
        }
        File.WriteAllLines(appInfo, lines);
    }

    private static string? GetBraveVersion(string braveExe)
    {
        var info = FileVersionInfo.GetVersionInfo(braveExe);
        var raw = info.ProductVersion ?? info.FileVersion ?? "";
        var m = Regex.Match(raw, @"(\d+)\.(\d+)\.(\d+)\.(\d+)");
        if (m.Success) return $"{m.Groups[2].Value}.{m.Groups[3].Value}.{m.Groups[4].Value}";
        m = Regex.Match(raw, @"(\d+\.\d+\.\d+)");
        return m.Success ? m.Groups[1].Value : null;
    }

    private static int CompareVersion(string a, string b)
    {
        var pa = a.Split('.').Select(int.Parse).ToArray();
        var pb = b.Split('.').Select(int.Parse).ToArray();
        var len = Math.Max(pa.Length, pb.Length);
        for (var i = 0; i < len; i++)
        {
            var va = i < pa.Length ? pa[i] : 0;
            var vb = i < pb.Length ? pb[i] : 0;
            if (va != vb) return va.CompareTo(vb);
        }
        return 0;
    }

    private static HttpClient CreateClient()
    {
        var client = new HttpClient { Timeout = TimeSpan.FromMinutes(20) };
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("BraveNightlyPortable", "1.0"));
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return client;
    }

    private static int Launch(string root, string exe, string[] args)
    {
        var psi = new ProcessStartInfo(exe)
        {
            UseShellExecute = true,
            WorkingDirectory = root,
        };
        if (args.Length > 0) psi.Arguments = string.Join(" ", args.Select(a => a.Contains(' ') ? $"\"{a}\"" : a));
        Process.Start(psi);
        return 0;
    }

    private sealed record ReleaseInfo(string Version, string AssetName, string DownloadUrl);
}

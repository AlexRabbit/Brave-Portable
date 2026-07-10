using System.Diagnostics;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BraveNightlyPortable;

internal static class Program
{
    private const string ChannelKeyword = "Nightly";
    private const string GitHubReleasesApi = "https://api.github.com/repos/brave/brave-browser/releases?per_page=100";
    private const string GitHubReleasesPage = "https://github.com/brave/brave-browser/releases";

    [STAThread]
    private static int Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        var root = FindPackageRoot();
        if (root is null)
        {
            MessageBox.Show(
                "Could not find App\\AppInfo\\appinfo.ini.\nKeep the folder structure intact.",
                "Brave Nightly Portable", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return 1;
        }

        if (!EnsureBraveReady(root))
        {
            MessageBox.Show(
                "Could not download Brave Nightly from official GitHub.\n\n" +
                "Check your internet connection and try again.\n" +
                "Manual fix: run Other\\Source\\update.bat",
                "Brave Nightly Portable", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return 1;
        }

        // Silent update check when already installed
        try { TryUpdate(root, forceIfMissing: false); } catch { /* launch existing version */ }

        var internalLauncher = Path.Combine(root, "BraveNightlyPortable-Internal.exe");
        if (!File.Exists(internalLauncher))
        {
            MessageBox.Show(
                $"Missing PA launcher:\n{internalLauncher}",
                "Brave Nightly Portable", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return 1;
        }

        Launch(root, internalLauncher, args);
        return 0;
    }

    private static string? FindPackageRoot()
    {
        var start = Path.GetDirectoryName(Environment.ProcessPath ?? AppContext.BaseDirectory);
        if (string.IsNullOrEmpty(start)) return null;

        var dir = start.TrimEnd(Path.DirectorySeparatorChar);
        for (var i = 0; i < 8; i++)
        {
            if (File.Exists(Path.Combine(dir, "App", "AppInfo", "appinfo.ini"))) return dir;
            var parent = Directory.GetParent(dir)?.FullName;
            if (string.IsNullOrEmpty(parent) || parent == dir) break;
            dir = parent;
        }
        return null;
    }

    private static bool EnsureBraveReady(string root)
    {
        var braveExe = Path.Combine(root, "App", "Brave", "brave.exe");
        if (File.Exists(braveExe)) return true;

        try
        {
            if (TryUpdate(root, forceIfMissing: true)) return File.Exists(braveExe);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        // PowerShell fallback (same logic, official GitHub source)
        try
        {
            var ps1 = Path.Combine(root, "Other", "Source", "Update-BraveNightly.ps1");
            if (File.Exists(ps1))
            {
                var psi = new ProcessStartInfo("powershell.exe",
                    $"-NoProfile -ExecutionPolicy Bypass -File \"{ps1}\" -Force")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = root,
                };
                using var p = Process.Start(psi);
                p?.WaitForExit(600000);
            }
        }
        catch { /* fall through */ }

        return File.Exists(braveExe);
    }

    private static bool TryUpdate(string root, bool forceIfMissing)
    {
        var braveDir = Path.Combine(root, "App", "Brave");
        var braveExe = Path.Combine(braveDir, "brave.exe");
        var needsInstall = !File.Exists(braveExe);

        if (!needsInstall && !forceIfMissing)
        {
            var current = GetBraveVersion(braveExe);
            var latest = GetLatestNightlyRelease() ?? GetLatestNightlyFromHtml();
            if (latest is null) return true;
            if (current is not null && CompareVersion(current, latest.Version) >= 0) return true;
            DownloadAndInstall(root, braveDir, latest);
            return File.Exists(braveExe);
        }

        if (needsInstall)
        {
            var latest = GetLatestNightlyRelease() ?? GetLatestNightlyFromHtml();
            if (latest is null) return false;
            DownloadAndInstall(root, braveDir, latest);
            return File.Exists(braveExe);
        }

        return true;
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
                if (!(nameEl.GetString() ?? "").Contains(ChannelKeyword, StringComparison.OrdinalIgnoreCase)) continue;
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
        catch { return null; }
        return null;
    }

    private static ReleaseInfo? GetLatestNightlyFromHtml()
    {
        using var client = CreateClient();
        try
        {
            var html = client.GetStringAsync(GitHubReleasesPage).GetAwaiter().GetResult();
            foreach (Match tagMatch in Regex.Matches(html, @"Nightly v(\d+\.\d+\.\d+)", RegexOptions.IgnoreCase))
            {
                var version = tagMatch.Groups[1].Value;
                var assetName = $"brave-v{version}-win32-x64.zip";
                return new ReleaseInfo(version, assetName,
                    $"https://github.com/brave/brave-browser/releases/download/v{version}/{assetName}");
            }
        }
        catch { return null; }
        return null;
    }

    private static void DownloadAndInstall(string root, string braveDir, ReleaseInfo release)
    {
        Directory.CreateDirectory(braveDir);
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
                try { proc.Kill(); } catch { }
            }
            Thread.Sleep(500);

            Directory.CreateDirectory(staging);
            ZipFile.ExtractToDirectory(zipPath, staging);

            var extractedBrave = FindBraveExe(staging)
                ?? throw new InvalidOperationException("Downloaded package missing brave.exe");

            VerifyNightlyBuild(extractedBrave);
            CopyDirectory(Path.GetDirectoryName(extractedBrave)!, braveDir);
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
        var product = FileVersionInfo.GetVersionInfo(braveExe).ProductName ?? "";
        if (!product.Contains("Nightly", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException($"Not Nightly: {product}");
    }

    private static string? FindBraveExe(string dir)
    {
        var direct = Path.Combine(dir, "brave.exe");
        if (File.Exists(direct)) return direct;
        foreach (var sub in Directory.EnumerateDirectories(dir))
        {
            var c = Path.Combine(sub, "brave.exe");
            if (File.Exists(c)) return c;
        }
        return null;
    }

    private static void CopyDirectory(string source, string dest)
    {
        if (Directory.Exists(dest))
        {
            foreach (var entry in Directory.EnumerateFileSystemEntries(dest))
            {
                try
                {
                    if (Directory.Exists(entry)) Directory.Delete(entry, true);
                    else File.Delete(entry);
                }
                catch { }
            }
        }
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
        var pkg = parts.Length >= 3 ? $"{parts[0]}.{parts[1]}.{parts[2]}.0" : "0.0.0.0";
        var lines = File.ReadAllLines(appInfo);
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("PackageVersion=", StringComparison.Ordinal))
                lines[i] = $"PackageVersion={pkg}";
            else if (lines[i].StartsWith("DisplayVersion=", StringComparison.Ordinal))
                lines[i] = $"DisplayVersion={version} Nightly";
        }
        File.WriteAllLines(appInfo, lines);
    }

    private static string? GetBraveVersion(string braveExe)
    {
        var raw = FileVersionInfo.GetVersionInfo(braveExe).ProductVersion ?? "";
        var m = Regex.Match(raw, @"(\d+)\.(\d+)\.(\d+)\.(\d+)");
        if (m.Success) return $"{m.Groups[2].Value}.{m.Groups[3].Value}.{m.Groups[4].Value}";
        m = Regex.Match(raw, @"(\d+\.\d+\.\d+)");
        return m.Success ? m.Groups[1].Value : null;
    }

    private static int CompareVersion(string a, string b)
    {
        var pa = a.Split('.').Select(int.Parse).ToArray();
        var pb = b.Split('.').Select(int.Parse).ToArray();
        for (var i = 0; i < Math.Max(pa.Length, pb.Length); i++)
        {
            var va = i < pa.Length ? pa[i] : 0;
            var vb = i < pb.Length ? pb[i] : 0;
            if (va != vb) return va.CompareTo(vb);
        }
        return 0;
    }

    private static HttpClient CreateClient()
    {
        var client = new HttpClient { Timeout = TimeSpan.FromMinutes(25) };
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("BraveNightlyPortable", "1.0"));
        return client;
    }

    private static void Launch(string root, string exe, string[] args)
    {
        var psi = new ProcessStartInfo(exe)
        {
            UseShellExecute = true,
            WorkingDirectory = root,
        };
        if (args.Length > 0)
            psi.Arguments = string.Join(" ", args.Select(a => a.Contains(' ') ? $"\"{a}\"" : a));
        Process.Start(psi);
    }

    private sealed record ReleaseInfo(string Version, string AssetName, string DownloadUrl);
}

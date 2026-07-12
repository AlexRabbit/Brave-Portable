using System.Diagnostics;
using Microsoft.Win32;

namespace BraveNightlyPortable;

/// <summary>
/// Registers BraveNightlyPortable-AlexRabbit.exe as a Windows browser so links
/// open the portable wrapper (correct profile) instead of App\Brave\brave.exe.
/// </summary>
internal static class DefaultBrowser
{
    private const string AppId = "BraveNightlyPortable.AlexRabbit";
    private const string ClientName = "Brave Nightly Portable (AlexRabbit)";
    private const string ProgIdHtml = "BraveNightlyPortable.AlexRabbit.HTML";
    private const string ProgIdUrl = "BraveNightlyPortable.AlexRabbit.URL";

    public static void Register(string wrapperExe)
    {
        var exe = Path.GetFullPath(wrapperExe);
        var icon = $"\"{exe}\",0";
        var openCmd = $"\"{exe}\" \"%1\"";

        using (var client = Registry.CurrentUser.CreateSubKey($@"Software\Clients\StartMenuInternet\{AppId}"))
        {
            client.SetValue("", ClientName);
            client.CreateSubKey("DefaultIcon")!.SetValue("", icon);
            using (var cmd = client.CreateSubKey(@"shell\open\command"))
                cmd!.SetValue("", $"\"{exe}\"");

            using (var caps = client.CreateSubKey("Capabilities"))
            {
                caps.SetValue("ApplicationName", ClientName);
                caps.SetValue("ApplicationDescription",
                    "Portable Brave Nightly by AlexRabbit — profile stays in the app folder.");
                caps.SetValue("ApplicationIcon", icon);

                using (var files = caps.CreateSubKey("FileAssociations"))
                {
                    files.SetValue(".htm", ProgIdHtml);
                    files.SetValue(".html", ProgIdHtml);
                    files.SetValue(".shtml", ProgIdHtml);
                    files.SetValue(".xht", ProgIdHtml);
                    files.SetValue(".xhtml", ProgIdHtml);
                }

                using (var urls = caps.CreateSubKey("URLAssociations"))
                {
                    urls.SetValue("http", ProgIdUrl);
                    urls.SetValue("https", ProgIdUrl);
                }
            }
        }

        using (var registered = Registry.CurrentUser.CreateSubKey(@"Software\RegisteredApplications"))
            registered.SetValue(ClientName, $@"Software\Clients\StartMenuInternet\{AppId}\Capabilities");

        WriteProgId(ProgIdHtml, "Brave Nightly Portable HTML Document", icon, openCmd);
        WriteProgId(ProgIdUrl, "Brave Nightly Portable URL", icon, openCmd);

        using (var http = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{ProgIdUrl}\URL Protocol"))
            http.SetValue("", "");

        // App Paths — helps some apps find this browser by name
        using (var appPath = Registry.CurrentUser.CreateSubKey(
                   $@"Software\Microsoft\Windows\CurrentVersion\App Paths\BraveNightlyPortable-AlexRabbit.exe"))
        {
            appPath.SetValue("", exe);
            appPath.SetValue("Path", Path.GetDirectoryName(exe)!);
        }
    }

    public static void Unregister()
    {
        try { Registry.CurrentUser.DeleteSubKeyTree($@"Software\Clients\StartMenuInternet\{AppId}", false); } catch { }
        try
        {
            using var registered = Registry.CurrentUser.OpenSubKey(@"Software\RegisteredApplications", writable: true);
            registered?.DeleteValue(ClientName, false);
        }
        catch { }
        try { Registry.CurrentUser.DeleteSubKeyTree($@"Software\Classes\{ProgIdHtml}", false); } catch { }
        try { Registry.CurrentUser.DeleteSubKeyTree($@"Software\Classes\{ProgIdUrl}", false); } catch { }
        try
        {
            Registry.CurrentUser.DeleteSubKeyTree(
                @"Software\Microsoft\Windows\CurrentVersion\App Paths\BraveNightlyPortable-AlexRabbit.exe", false);
        }
        catch { }
    }

    public static void OpenWindowsDefaultAppsSettings()
    {
        try
        {
            Process.Start(new ProcessStartInfo("ms-settings:defaultapps") { UseShellExecute = true });
        }
        catch
        {
            try
            {
                Process.Start(new ProcessStartInfo("control.exe", "/name Microsoft.DefaultPrograms")
                {
                    UseShellExecute = true,
                });
            }
            catch { /* ignore */ }
        }
    }

    private static void WriteProgId(string progId, string friendlyName, string icon, string openCmd)
    {
        using var key = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{progId}");
        key.SetValue("", friendlyName);
        key.CreateSubKey("DefaultIcon")!.SetValue("", icon);
        key.CreateSubKey(@"shell\open\command")!.SetValue("", openCmd);
    }
}

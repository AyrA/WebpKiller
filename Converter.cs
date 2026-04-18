using System.Diagnostics;

namespace WebpKiller;

internal static class Converter
{
    public static bool HasMagick()
    {
        var m = FileFinder.FindExact("magick.exe");
        if (m == null)
        {
            return false;
        }
        try
        {
            using var p = Magick("--version");
            p.WaitForExit();
            return p.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    public static Task<bool> Convert(string webp)
    {
        return Convert(webp, Path.ChangeExtension(webp, ".jpg"));
    }

    public static async Task<bool> Convert(string webp, string jpg)
    {
        if (!File.Exists(webp))
        {
            return false;
        }
        using var p = Magick(webp, jpg);
        await p.WaitForExitAsync();
        return p.ExitCode == 0 && File.Exists(jpg);
    }

    private static Process Magick(params object[] args)
    {
        var psi = new ProcessStartInfo()
        {
            FileName = FileFinder.FindExact("magick.exe") ?? throw new InvalidOperationException("Magick could not be found"),
            UseShellExecute = false,
            CreateNoWindow = true
        };
        foreach (var arg in args)
        {
            psi.ArgumentList.Add(arg.ToString() ?? "");
        }
        return Process.Start(psi)
            ?? throw new Exception("Unable to start a new process");
    }
}

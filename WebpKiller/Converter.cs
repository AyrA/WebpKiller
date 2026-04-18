using System.Diagnostics;

namespace WebpKiller;

/// <summary>
/// Handles imagemagick interaction
/// </summary>
internal static class Converter
{
    /// <summary>
    /// Checks if magick.exe can be found on the system
    /// </summary>
    /// <returns>true if found, false otherwise</returns>
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

    /// <summary>
    /// Converts a webp to jpeg
    /// </summary>
    /// <param name="webp">webp file</param>
    /// <returns>true if success, false otherwise</returns>
    /// <remarks>
    /// The output file name will be the same as the input, but with a jpeg extension.
    /// On failure, the output is not deleted, and may contain corrupted or no content.
    /// </remarks>
    public static Task<bool> Convert(string webp)
    {
        return Convert(webp, Path.ChangeExtension(webp, ".jpg"));
    }

    /// <summary>
    /// Converts a webp to jpeg
    /// </summary>
    /// <param name="webp">webp file</param>
    /// <param name="jpg">Destination file</param>
    /// <returns>true if success, false otherwise</returns>
    /// <remarks>
    /// On failure, the output is not deleted, and may contain corrupted or no content.
    /// </remarks>
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

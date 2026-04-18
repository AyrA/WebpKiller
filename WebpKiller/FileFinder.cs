namespace WebpKiller;

/// <summary>
/// Finds the full path of an executable that is invoked by simply typing its name without path information
/// </summary>
internal static class FileFinder
{
    private static readonly string[] exts;
    private static readonly string[] dirs;

    static FileFinder()
    {
        exts = (Environment.GetEnvironmentVariable("PATHEXT") ?? ".COM;.EXE;.BAT;.CMD;.VBS;.VBE").Split(';');
        dirs = [Path.GetFullPath(Environment.CurrentDirectory), .. (Environment.GetEnvironmentVariable("PATH") ?? "").Split(';').Select(Path.GetFullPath)];
    }

    /// <summary>
    /// Find the full path of the supplied executable
    /// </summary>
    /// <param name="file">Executable name (without extension)</param>
    /// <returns>Full path of the executable, or null if not found</returns>
    public static string? Find(string file)
    {
        //Only apply extensions if the supplied file name does not contain any of them
        if (exts.Any(m => file.EndsWith(m, StringComparison.InvariantCultureIgnoreCase)))
        {
            return FindExact(file);
        }
        foreach (var dir in dirs)
        {
            foreach (var ext in exts)
            {
                var name = file + ext;
                var p = Path.Combine(dir, name);
                if (File.Exists(p))
                {
                    return Path.GetFullPath(p);
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Finds the path of an executable.
    /// This call does not try file extension substitution.
    /// To also search for file extensions, use <see cref="Find(string)"/>
    /// </summary>
    /// <param name="file">Executable name (with extension)</param>
    /// <returns>Full path of the executable, or null if not found</returns>
    public static string? FindExact(string file)
    {
        foreach (var dir in dirs)
        {
            var p = Path.Combine(dir, file);
            if (File.Exists(p))
            {
                return Path.GetFullPath(p);
            }
        }
        return null;
    }
}

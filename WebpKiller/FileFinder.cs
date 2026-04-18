namespace WebpKiller;

internal static class FileFinder
{
    private static readonly string[] exts;
    private static readonly string[] dirs;

    static FileFinder()
    {
        exts = (Environment.GetEnvironmentVariable("PATHEXT") ?? ".COM;.EXE;.BAT;.CMD;.VBS;.VBE").Split(';');
        dirs = [Path.GetFullPath(Environment.CurrentDirectory), .. (Environment.GetEnvironmentVariable("PATH") ?? "").Split(';').Select(Path.GetFullPath)];
    }

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
                    return p;
                }
            }
        }
        return null;
    }

    public static string? FindExact(string file)
    {
        foreach (var dir in dirs)
        {
            var p = Path.Combine(dir, file);
            if (File.Exists(p))
            {
                return p;
            }
        }
        return null;
    }
}

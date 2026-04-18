using System.Reflection;

namespace WebpKiller;

internal static class Resources
{
    public static Icon GetApplicationIcon()
    {
        using var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("WebpKiller.icon.ico")
            ?? throw new IOException("Local resource 'icon.ico' not found");
        return new Icon(s);
    }
}

using System.Reflection;

namespace WebpKiller;

/// <summary>
/// Provides easier access to internal resources
/// </summary>
internal static class Resources
{
    /// <summary>
    /// Gets the application icon.
    /// </summary>
    /// <returns>Icon resource. It's the callers responsibility to dispose of this</returns>
    /// <exception cref="IOException">Unable to load icon resource</exception>
    /// <remarks>
    /// This method returns a completely new reference each time it is invoked
    /// </remarks>
    public static Icon GetApplicationIcon()
    {
        using var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("WebpKiller.icon.ico")
            ?? throw new IOException("Local resource 'icon.ico' not found");
        return new Icon(s);
    }
}

using ImageMagick;
using System.Diagnostics;

namespace WebpKiller;

internal static class InternalConvert
{
    static InternalConvert()
    {
        MagickNET.Initialize();
    }

    public static Task<bool> Convert(string webp)
        => Convert(webp, Path.ChangeExtension(webp, ".jpg"));

    public static async Task<bool> Convert(string webp, string jpeg)
    {
        try
        {
            using var image = new MagickImage(File.ReadAllBytes(webp));
            if (!image.IsOpaque)
            {
                Debug.Print("Maybe losing transparency by converting {0}", webp);
            }
            await image.WriteAsync(jpeg, MagickFormat.Jpeg);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
namespace WebpKiller;

internal static class ConvertWithRetry
{
    public static Task<bool> Convert(string webpFile, int retryCount, TimeSpan delay)
        => Convert(webpFile, retryCount, (int)delay.TotalMilliseconds);

    public static async Task<bool> Convert(string webpFile, int retryCount, int delayMilliseconds)
    {
        ArgumentException.ThrowIfNullOrEmpty(webpFile);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(retryCount);
        ArgumentOutOfRangeException.ThrowIfNegative(delayMilliseconds);
        do
        {
            try
            {
                if (await Converter.Convert(webpFile))
                {
                    return true;
                }
            }
            catch
            {
                //NOOP
            }
            if (delayMilliseconds > 0)
            {
                await Task.Delay(delayMilliseconds);
            }
        } while (--retryCount > 0);
        return false;
    }
}

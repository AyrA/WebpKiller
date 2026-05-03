namespace WebpKiller;

/// <summary>
/// A wrapper around <see cref="Converter"/> with retry handling
/// </summary>
internal static class ConvertWithRetry
{
    /// <summary>
    /// Convert a file
    /// </summary>
    /// <param name="webpFile">webp file</param>
    /// <param name="retryCount">Maximum number of attempts</param>
    /// <param name="delay">Delay between failures</param>
    /// <returns>true if final result was success, false otherwise</returns>
    public static Task<bool> Convert(string webpFile, int retryCount, TimeSpan delay)
        => Convert(webpFile, retryCount, (int)delay.TotalMilliseconds);

    /// <summary>
    /// Convert a file
    /// </summary>
    /// <param name="webpFile">webp file</param>
    /// <param name="retryCount">Maximum number of attempts</param>
    /// <param name="delayMilliseconds">Delay between failures</param>
    /// <returns>true if final result was success, false otherwise</returns>
    public static async Task<bool> Convert(string webpFile, int retryCount, int delayMilliseconds)
    {
        ArgumentException.ThrowIfNullOrEmpty(webpFile);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(retryCount);
        ArgumentOutOfRangeException.ThrowIfNegative(delayMilliseconds);

        do
        {
            try
            {
                if (await InternalConvert.Convert(webpFile))
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

using System.Diagnostics;
using WebpKiller.Settings;

namespace WebpKiller;

/// <summary>
/// Monitors folders for webp files and automatically takes actions based on the folder configuration
/// </summary>
internal static class AutoMonitor
{
    private static readonly List<string> processing = [];
    private static readonly Watcher watcher;
    private static readonly Dictionary<int, FolderSettingsV1> settingsRef = [];
    static bool running = false;

    static AutoMonitor()
    {
        watcher = new();
        watcher.FileCreated += Watcher_FileCreated;
    }

    /// <summary>
    /// Does a full directory scan for all settings that are configured to scan at startup.
    /// </summary>
    /// <param name="cancellationToken">Token to gracefully cancel the startup scan</param>
    /// <returns>Thread of the startup scan</returns>
    public static Thread AutoScan(CancellationToken cancellationToken = default)
        => AutoScan(SettingsProvider.GetSettings().Settings, cancellationToken);

    /// <summary>
    /// Does a full directory scan for all settings that are configured to scan at startup.
    /// </summary>
    /// <param name="settings">
    /// Settings to process. Entries are only considered if they have <see cref="FolderSettingsV1.ScanOnStart"/> set to true
    /// and <see cref="FolderSettingsV1.Enabled"/> set to true
    /// </param>
    /// <param name="cancellationToken">Provides means to gracefully stop the conversion</param>
    /// <returns>Thread of the startup scan</returns>
    public static Thread AutoScan(FolderSettingsV1[] settings, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(settings);

        var t = new Thread(() =>
        {
            using var sem = new SemaphoreSlim(Math.Max(1, Environment.ProcessorCount - 1));
            async Task AsyncConvert(string file, FolderSettingsV1 setting)
            {
                try
                {
                    var result = await ConvertWithRetry.Convert(file, 10, 1000);
                    if (result && setting.DeleteWebp)
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch
                        {
                            //NOOP
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Print("AsyncConvert({0}): {1}", file, ex.Message);
                }
                finally
                {
                    sem.Release();
                }
            }

            foreach (var setting in settings)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                if (setting.Enabled && setting.ScanOnStart)
                {
                    var enumOpt = new EnumerationOptions()
                    {
                        IgnoreInaccessible = true,
                        RecurseSubdirectories = setting.Recursive
                    };

                    foreach (var f in Directory.EnumerateFiles(setting.Folder, "*.webp", enumOpt))
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }
                        sem.Wait();
                        new Thread(() => _ = AsyncConvert(f, setting)).Start();
                    }
                }
            }
            //Take all semaphores to ensure that all conversion processes have completed
            var proc = Math.Max(1, Environment.ProcessorCount - 1);
            for (var i = 0; i < proc; i++)
            {
                sem.Wait();
            }
        });
        t.Start();
        return t;
    }

    /// <summary>
    /// Starts (or restarts) directory monitoring and file conversion
    /// based on the settings from the default settings provider
    /// </summary>
    public static void Start()
        => Start(SettingsProvider.GetSettings().Settings);

    /// <summary>
    /// Starts (or restarts) directory monitoring and file conversion based on the supplied settings
    /// </summary>
    /// <param name="settings">Settings to apply. An empty array is like calling <see cref="Stop"/></param>
    public static void Start(FolderSettingsV1[] settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        lock (settingsRef)
        {
            Stop();
            foreach (var setting in settings)
            {
                var id = watcher.MonitorPath(setting.Folder, setting.Recursive);
                settingsRef[id] = setting;
            }
            running = true;
        }
    }

    /// <summary>
    /// Stops all monitoring
    /// </summary>
    public static void Stop()
    {
        lock (settingsRef)
        {
            running = false;
            watcher.StopAll();
            settingsRef.Clear();
        }
    }

    private static async void Watcher_FileCreated(object sender, FileCreatedEventArgs e)
    {
        if (running)
        {
            if (settingsRef.TryGetValue(e.Id, out var setting))
            {
                //Suppress duplicate events by temporarily storing files currently being processed
                lock (processing)
                {
                    if (processing.Contains(e.Path))
                    {
                        return;
                    }
                    processing.Add(e.Path);
                }
                var result = await ConvertWithRetry.Convert(e.Path, 10, 1000);
                lock (processing)
                {
                    processing.Remove(e.Path);
                }
                if (result && setting.DeleteWebp)
                {
                    try
                    {
                        File.Delete(e.Path);
                    }
                    catch
                    {
                        //NOOP
                    }
                }
                if (setting.ShowConversionMsg)
                {
                    Program.ReportConversionResult(e.Path, result);
                }
            }
        }
    }
}

namespace WebpKiller;

/// <summary>
/// Watches a directory for newly created webp files
/// </summary>
internal class Watcher : IDisposable
{
    /// <summary>
    /// Event that is raised every time a webp is created
    /// </summary>
    /// <remarks>
    /// Depending on how the creating application creates the webp,
    /// this event may be raised multiple times for the same file,
    /// and you may need a duplication detection at the event handler
    /// </remarks>
    public event FileCreatedHandler FileCreated = delegate { };

    private readonly Dictionary<int, FileSystemWatcher> watchers = [];
    private int index = 0;
    private bool disposed;

    /// <summary>
    /// Monitors the given path for webp files
    /// </summary>
    /// <param name="path">Path to monitor</param>
    /// <param name="recursive">Include subdirectories</param>
    /// <returns>Monitoring id, used in other calls like <see cref="StopMonitor(int)"/></returns>
    /// <exception cref="DirectoryNotFoundException"><paramref name="path"/> is invalid</exception>
    public int MonitorPath(string path, bool recursive)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        ObjectDisposedException.ThrowIf(disposed, this);
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException("Directory not found: " + path);
        }
        var watcher = new FileSystemWatcher(Path.GetFullPath(path))
        {
            Filter = "*.webp",
            IncludeSubdirectories = recursive,
            NotifyFilter = NotifyFilters.FileName
        };
        lock (watchers)
        {
            watchers[++index] = watcher;
        }

        watcher.Created += Watcher_Created;
        watcher.EnableRaisingEvents = true;
        return index;
    }

    /// <summary>
    /// Stops a given monitor
    /// </summary>
    /// <param name="monitorId">Monitor id</param>
    /// <returns>true if found and stopped, false if not found</returns>
    public bool StopMonitor(int monitorId)
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        FileSystemWatcher? watcher;
        lock (watchers)
        {
            if (watchers.TryGetValue(monitorId, out watcher))
            {
                watchers.Remove(monitorId);
            }
        }
        if (watcher != null)
        {
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Stops all monitors
    /// </summary>
    public void StopAll()
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        foreach (var kv in watchers)
        {
            kv.Value.EnableRaisingEvents = false;
            kv.Value.Dispose();
        }
        watchers.Clear();
    }

    /// <summary>
    /// Gets all currently active monitor configurations
    /// </summary>
    /// <returns></returns>
    public MonitorInfo[] GetMonitor()
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        lock (watchers)
        {
            return [.. watchers.Select(m => new MonitorInfo(m.Key, m.Value.Path))];
        }
    }

    private void Watcher_Created(object sender, FileSystemEventArgs e)
    {
        if (sender is FileSystemWatcher w)
        {
            var id = watchers.First(m => m.Value == w).Key;
            FileCreated(this, new(id, e.FullPath));
        }
    }

    public void Dispose()
    {
        disposed = true;
        GC.SuppressFinalize(this);
        lock (watchers)
        {
            foreach (var w in watchers)
            {
                w.Value.EnableRaisingEvents = false;
                w.Value.Dispose();
            }
            watchers.Clear();
        }
    }
}

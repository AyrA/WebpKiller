namespace WebpKiller;

internal class Watcher : IDisposable
{
    public event FileCreatedHandler FileCreated = delegate { };

    private readonly Dictionary<int, FileSystemWatcher> watchers = [];
    private int index = 0;
    private bool disposed;

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

    public bool StopMonitor(int index)
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        FileSystemWatcher? watcher;
        lock (watchers)
        {
            if (watchers.TryGetValue(index, out watcher))
            {
                watchers.Remove(index);
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

using System.Diagnostics;

FileSystemWatcher[] watchers;
bool idle = false;
Application.Idle += Application_Idle;

void Application_Idle(object? sender, EventArgs e)
{
    if (!idle)
    {
        idle = true;
        if (args.Length == 0)
        {
            MessageBox.Show("WebpKiller <dir> [dir ...]");
            return;
        }
        else
        {
            watchers = [.. args.Select(m => new FileSystemWatcher()
            {
                Path = m,
                Filter = "*.webp",
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.FileName
            })];
            foreach (var w in watchers)
            {
                Console.WriteLine("Setting up watcher for {0}", w.Path);
                w.Created += FileCreated;
                w.EnableRaisingEvents = true;
                foreach (var f in Directory.EnumerateFiles(w.Path, "*.webp", SearchOption.AllDirectories))
                {
                    Convert(f, 2, 1000);
                }
            }
        }
    }
}

Application.Run();

static async void Convert(string path, int attempts, int delay)
{
    if (!File.Exists(path))
    {
        return;
    }
    var dest = Path.ChangeExtension(path, ".jpg");
    var psi = new ProcessStartInfo()
    {
        FileName = "magick",
        UseShellExecute = false,
        CreateNoWindow = true
    };
    psi.ArgumentList.Add(path);
    psi.ArgumentList.Add(dest);
    using var p = Process.Start(psi);
    await p.WaitForExitAsync();
    if (p.ExitCode == 0 && File.Exists(dest))
    {
        File.Delete(path);
    }
    else if (attempts > 1)
    {
        await Task.Delay(delay);
        Convert(path, attempts - 1, delay);
    }
    else
    {
        Console.WriteLine("Failed to convert {0}", path);
    }
}

static void FileCreated(object sender, FileSystemEventArgs e)
{
    Convert(e.FullPath, 10, 1000);
}
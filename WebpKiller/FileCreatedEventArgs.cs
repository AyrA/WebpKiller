namespace WebpKiller;

/// <summary>
/// Event handler for when a webp file was created
/// </summary>
/// <param name="id">Watcher id</param>
/// <param name="path">Absolute file path of webp file</param>
public class FileCreatedEventArgs(int id, string path) : EventArgs
{
    public int Id { get; } = id;

    public string Path { get; } = path;
}


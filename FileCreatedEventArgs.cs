namespace WebpKiller;

public class FileCreatedEventArgs(int id, string path) : EventArgs
{
    public int Id { get; } = id;

    public string Path { get; } = path;
}


namespace WebpKiller;

/// <summary>
/// Record to keep track of paths being monitored
/// </summary>
/// <param name="Id">Monitor id</param>
/// <param name="FullPath">Monitored path</param>
public record MonitorInfo(int Id, string FullPath);

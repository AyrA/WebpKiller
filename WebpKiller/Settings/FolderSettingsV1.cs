namespace WebpKiller.Settings;

/// <summary>
/// Folder settings version 1
/// </summary>
/// <param name="Folder">Folder path</param>
/// <param name="Enabled">Conversion enabled or not</param>
/// <param name="Recursive">Perform recursive scans</param>
/// <param name="ScanOnStart">Scan for webp during application start</param>
/// <param name="DeleteWebp">Delete webp after successful conversion</param>
/// <param name="ShowConversionMsg">Show conversion success or failure message</param>
public record FolderSettingsV1(string Folder, bool Enabled, bool Recursive, bool ScanOnStart, bool DeleteWebp, bool ShowConversionMsg) : IUpgradeable
{
    public IUpgradeable Upgrade()
    {
        return this;
    }
}

namespace WebpKiller.Settings;

public record FolderSettingsV1(string Folder, bool Enabled, bool Recursive, bool ScanOnStart, bool DeleteWebp, bool ShowConversionMsg) : IUpgradeable
{
    public IUpgradeable Upgrade()
    {
        return this;
    }
}

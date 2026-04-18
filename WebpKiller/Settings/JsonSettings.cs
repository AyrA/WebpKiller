namespace WebpKiller.Settings;

public record JsonSettingsV1(int Version, FolderSettingsV1[] Settings) : BaseJsonSettings(Version), IUpgradeable
{
    public IUpgradeable Upgrade()
    {
        return this;
    }
}
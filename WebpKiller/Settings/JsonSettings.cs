namespace WebpKiller.Settings;

/// <summary>
/// Current json settings model
/// </summary>
/// <param name="Version">Settings version</param>
/// <param name="Settings">Folder settings</param>
public record JsonSettingsV1(int Version, FolderSettingsV1[] Settings) : BaseJsonSettings(Version), IUpgradeable
{
    public IUpgradeable Upgrade()
    {
        return this;
    }
}
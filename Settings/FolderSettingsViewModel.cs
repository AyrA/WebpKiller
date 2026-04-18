using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebpKiller.Settings;

public class FolderSettingsViewModel(FolderSettingsV1 settings)
{
    public FolderSettingsV1 Settings { get; set; } = settings;

    [MemberNotNull(nameof(Settings))]
    public void Validate()
    {
        if (Settings == null)
        {
            throw new ValidationException("Settings cannot be empty");
        }
        if (!Directory.Exists(Settings.Folder))
        {
            throw new DirectoryNotFoundException($"Directory '{Settings.Folder}' does not exist");
        }
    }

    public override string ToString() => Settings.Folder;
}

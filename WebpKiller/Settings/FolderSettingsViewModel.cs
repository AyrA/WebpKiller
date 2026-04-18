using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebpKiller.Settings;

/// <summary>
/// View model for the folder settings form
/// </summary>
/// <param name="settings">Folder setting</param>
public class FolderSettingsViewModel(FolderSettingsV1 settings)
{
    /// <summary>
    /// Gets or sets the setting
    /// </summary>
    public FolderSettingsV1 Settings { get; set; } = settings;

    /// <summary>
    /// Validates <see cref="Settings"/>
    /// </summary>
    /// <exception cref="ValidationException">Settings not set</exception>
    /// <exception cref="DirectoryNotFoundException">Configured directory was not found</exception>
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

    /// <summary>
    /// Gets the string used for display purposes
    /// </summary>
    /// <returns>Value of <see cref="FolderSettingsV1.Folder"/></returns>
    public override string ToString() => Settings.Folder;
}

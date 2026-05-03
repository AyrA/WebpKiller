using System.ComponentModel.DataAnnotations;

namespace WebpKiller.Settings;

/// <summary>
/// Current json settings model
/// </summary>
/// <param name="Version">Settings version</param>
/// <param name="Settings">Folder settings</param>
public record JsonSettingsV1(int Version, FolderSettingsV1[] Settings) : BaseJsonSettings(Version), IUpgradeable, IValidatableObject, IFixable
{
    public IUpgradeable Upgrade()
    {
        return this;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return Settings.SelectMany(m => m.Validate(validationContext));
    }

    public ValidationResult[] Validate()
    {
        var ctx = new ValidationContext(this);
        return [.. Validate(ctx)];
    }

    public void Fix()
    {
        foreach (var folder in Settings)
        {
            folder.Fix();
        }
    }
}

public interface IFixable
{
    void Fix();
}
using System.ComponentModel.DataAnnotations;

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
public class FolderSettingsV1(string folder, bool enabled, bool recursive, bool scanOnStart, bool deleteWebp, bool showConversionMsg) : IUpgradeable, IValidatableObject, IFixable
{
    public string Folder { get; private set; } = folder;
    public bool Enabled { get; private set; } = enabled;
    public bool Recursive { get; private set; } = recursive;
    public bool ScanOnStart { get; private set; } = scanOnStart;
    public bool DeleteWebp { get; private set; } = deleteWebp;
    public bool ShowConversionMsg { get; private set; } = showConversionMsg;

    public IUpgradeable Upgrade()
    {
        return this;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        try
        {
            if (!Directory.Exists(Folder))
            {
                throw new DirectoryNotFoundException($"{Folder} does not exist");
            }
            //Enumerate files to check read access
            _ = Directory.EnumerateFiles(Folder, "*", new EnumerationOptions()
            {
                RecurseSubdirectories = true,
                MatchType = MatchType.Simple
            }).Any();
        }
        catch
        {
            return [new ValidationResult($"Folder does not exist or is inaccessible: '{Folder}'")];
        }
        return [];
    }

    public void Fix()
    {
        try
        {
            if (!Directory.Exists(Folder))
            {
                throw new DirectoryNotFoundException($"{Folder} does not exist");
            }
            //Enumerate files to check read access
            _ = Directory.EnumerateFiles(Folder, "*", new EnumerationOptions()
            {
                RecurseSubdirectories = true,
                MatchType = MatchType.Simple
            }).Any();
        }
        catch
        {
            Enabled = false;
            ScanOnStart = false;
        }
    }
}

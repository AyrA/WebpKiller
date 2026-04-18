namespace WebpKiller.Settings;

internal static class SettingsProvider
{
    public const int CurrentVersion = 1;

    private static readonly string settingsFile = Path.Combine(Application.StartupPath, "settings.json");

    private static readonly Dictionary<int, Type> versionMap = new()
    {
        { CurrentVersion, typeof(JsonSettingsV1) }
    };

    public static JsonSettingsV1 GetSettings()
    {
        try
        {
            var json = File.ReadAllText(settingsFile);
            var baseItem = System.Text.Json.JsonSerializer.Deserialize<BaseJsonSettings>(json)
                ?? throw new InvalidDataException("Invalid settings");
            var t = versionMap[baseItem.Version];
            var item = (IUpgradeable?)System.Text.Json.JsonSerializer.Deserialize(json, t)
                ?? throw new InvalidDataException("Settings type conversion failed");
            return (JsonSettingsV1)item.Upgrade();
        }
        catch
        {
            return new(versionMap.Keys.Max(), []);
        }
    }

    public static void SaveSettings(JsonSettingsV1 settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        if (settings.Version != CurrentVersion)
        {
            throw new ArgumentException("Given settings is not of the current version");
        }
        var json = System.Text.Json.JsonSerializer.Serialize(settings);
        File.WriteAllText(settingsFile, json);
    }
}

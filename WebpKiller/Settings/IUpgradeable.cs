namespace WebpKiller.Settings;

/// <summary>
/// Provides means to upgrade a model
/// </summary>
/// <typeparam name="T">Model to upgrade</typeparam>
public interface IUpgradeable<T> where T : IUpgradeable
{
    /// <summary>
    /// Upgrades this instance to the latest version
    /// </summary>
    /// <returns>Latest version. Current instance if it is already the latest version</returns>
    T Upgrade();
}

/// <summary>
/// Provides means to upgrade a model
/// </summary>
public interface IUpgradeable
{
    /// <summary>
    /// Upgrades this instance to the latest version
    /// </summary>
    /// <returns>Latest version. Current instance if it is already the latest version</returns>
    IUpgradeable Upgrade();
}
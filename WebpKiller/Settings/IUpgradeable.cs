namespace WebpKiller.Settings;

public interface IUpgradeable<T> where T : IUpgradeable
{
    T Upgrade();
}

public interface IUpgradeable
{
    IUpgradeable Upgrade();
}
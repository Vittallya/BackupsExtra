namespace Backups.FileSystem.Abstract
{
    public interface IVirtualDirectoryFactory<TDir>
        where TDir : class, IVirtualDirectory
    {
        TDir Create(TDir dir, string name);
        TDir Create(string fullPath);
    }
}

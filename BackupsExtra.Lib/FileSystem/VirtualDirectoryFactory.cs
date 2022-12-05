using Backups.FileSystem.Abstract;

namespace Backups.FileSystem
{
    public class VirtualDirectoryFactory : IVirtualDirectoryFactory<IVirtualDirectory>
    {
        public IVirtualDirectory Create(IVirtualDirectory dir, string name) => new VirtualDirectory(dir, name);

        public IVirtualDirectory Create(string fullPath) => new VirtualDirectory(fullPath);
    }
}

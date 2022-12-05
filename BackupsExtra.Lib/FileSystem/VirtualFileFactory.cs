using Backups.FileSystem.Abstract;
using System;
using System.IO;

namespace Backups.FileSystem
{
    public class VirtualFileFactory : IVirtualFileFactory<IVirtualDirectory, IVirtualFile>
    {
        public IVirtualFile Create(IVirtualDirectory dir, string name, Func<Stream> func) =>
            new VirtualFile(dir, name, func);
    }
}

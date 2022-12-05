using System.Collections.Generic;
using System.IO.Compression;

namespace Backups.FileSystem
{
    public interface IZipVisitor
    {
        public IEnumerable<ZipArchiveEntry> Entries { get; }
        public void Visit(IVirtualFile file, IVirtualDirectory directParent);
        public void Visit(IVirtualDirectory file, IVirtualDirectory directParent);
    }
}

using Backups.FileSystem;
using System.Collections.Generic;

namespace Backups
{
    public interface IZipArchiver
    {
        IEnumerable<string> Archive(IVirtualObject obj, string path, IRepository rep);
        IEnumerable<string> Archive(IEnumerable<IVirtualObject> obj, string path, IRepository rep);
    }
}
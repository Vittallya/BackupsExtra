using Backups.FileSystem;
using System.Collections.Generic;

namespace Backups
{
    public interface IArchiver
    {
        void Archive(IVirtualObject obj, string path, IRepository rep);
        void Archive(IEnumerable<IVirtualObject> obj, string path, IRepository rep);
    }
}
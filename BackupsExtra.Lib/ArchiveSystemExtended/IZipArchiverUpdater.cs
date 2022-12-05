using Backups;
using Backups.FileSystem;
using System.Collections.Generic;

namespace BackupsExtra.Lib.ArchiveSystemExtended
{
    public interface IZipArchiverUpdater: IZipArchiver
    {
        public void Update(IEnumerable<IVirtualObject> objects, string path, IRepository rep);
    }
}

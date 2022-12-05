using Backups.BackupSystem;
using Backups.FileSystem;
using System.Collections.Generic;

namespace Backups.ArchiveSystem
{
    public interface IStorage
    {
        public IEnumerable<BackupObject> Backups { get; }

        public IEnumerable<IVirtualObject> GetObjects();

        public IEnumerable<IVirtualObject> GetObjects(BackupObject obj);
    }
}

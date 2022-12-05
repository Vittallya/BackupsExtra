using Backups.ArchiveSystem;
using Backups.Comparers;
using Backups.FileSystem;
using System.Collections.Generic;
using System.Linq;

namespace Backups.BackupSystem
{
    internal class SplitStorage : IStorage
    {
        private readonly IEnumerable<ZipStorage> storages;

        public SplitStorage(IEnumerable<ZipStorage> storages)
        {
            this.storages = storages;
        }

        public IEnumerable<BackupObject> Backups => storages.SelectMany(x => x.Backups);

        public IEnumerable<IVirtualObject> GetObjects()
        {
            return storages.SelectMany(x => x.GetObjects());
        }

        public IEnumerable<IVirtualObject> GetObjects(BackupObject obj)
        {
            ZipStorage storage = storages.First(x => x.Backups.Contains(obj, new GenericComparer<BackupObject>(x => x.RelativePathToOrig)));
            return storage.GetObjects(obj);
        }
    }
}

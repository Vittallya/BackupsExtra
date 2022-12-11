using Backups.BackupSystem;
using Backups.Comparers;
using Backups.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackupsExtra.Lib.StorageSystem
{
    public class SplitStorageExtended : IStorageExtended
    {
        public SplitStorageExtended(IEnumerable<ZipStorageExtended> storages,
                                    string algorithmType,
                                    string backupDir,
                                    IRepository storageRepository,
                                    string rootPath)
        {
            Storages = storages;
            AlgorithmType = algorithmType;
            StorageRepository = storageRepository;
            BackupDir = backupDir;
            Path = rootPath;

            PathToArchives = storages.Select(x => x.Path);
            Backups = storages.SelectMany(x => x.Backups);
        }

        public IEnumerable<ZipStorageExtended> Storages { get; }

        public string AlgorithmType { get; }

        public IRepository StorageRepository { get; }

        public IEnumerable<string> PathToArchives {
            get;
        }

        public IEnumerable<BackupObject> Backups {
            get;
        }

        public string Path {
            get;
        }

        public string BackupDir { get; }

        public IEnumerable<IVirtualObject> GetObjects()
        {
            return Storages.SelectMany(x => x.GetObjects());
        }

        public IEnumerable<IVirtualObject> GetObjects(BackupObject obj)
        {
            var storage = Storages.First(x => x.Backups.Contains(obj, new GenericComparer<BackupObject>(x => x.RelativePathToOrig)));
            return storage.GetObjects(obj);
        }
    }
}

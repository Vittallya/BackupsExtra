using Backups.BackupSystem;
using Backups.Comparers;
using Backups.FileSystem;
using System.Collections.Generic;
using System.Linq;
using Backups.Algorithms;
using System;
using Backups.ErrorPool;
using BackupsExtra.Lib.BackupSystemExtended;
using BackupsExtra.Lib.StorageSystem;
using Backups.ArchiveSystem;
using BackupsExtra.Lib.Loggers;

namespace BackupsExtra.Lib.MergeSystem
{
    public class Merger : IMerger
    {
        private readonly IBackupTaskExtended backupTask;
        private readonly ILogger logger;

        public Merger(IBackupTaskExtended backupTask, ILogger logger)
        {
            this.backupTask = backupTask;
            this.logger = logger;
        }

        //для каждого IStorage нужны:
        //1- путь (или пути к архивам)
        //2- репозиторий архивов
        //3- 

        public void Merge(RestorePoint p1, RestorePoint p2)
        {
            IEnumerable<BackupObject> except = p1.Storage.Backups.
                Except(p2.Storage.Backups, new GenericComparer<BackupObject>(x => x.RelativePathToOrig));

            if (except.Any())
            {

                IEnumerable<IVirtualObject> exceptObjects = except.SelectMany(backupObj => p1.Storage.GetObjects(backupObj));
                IEnumerable<IVirtualObject> union = exceptObjects.Union(p2.Storage.GetObjects());

                IStorageExtended storage2 = (IStorageExtended)p2.Storage;

                ISaveAlgorithm algorithm = (ISaveAlgorithm)Activator.
                    CreateInstance(
                        Type.GetType(storage2.AlgorithmType),
                        new object[] { storage2.StorageRepository, storage2.BackupDir, new ZipArchiver(), new MockupLogger() });

                algorithm.MakeBackup(union, backupTask.Name, p2.Number.ToString(), null);
                logger.LogMessage($"From point {p1.Number} to point {p2.Number} was merged {except.Count()} objects");
            }
        }
    }
}

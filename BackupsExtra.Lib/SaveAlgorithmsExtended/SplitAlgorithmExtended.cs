using Backups;
using Backups.Algorithms.Abstract;
using Backups.ArchiveSystem;
using Backups.BackupSystem;
using Backups.ErrorPool;
using Backups.FileSystem;
using BackupsExtra.Lib.Extensions;
using BackupsExtra.Lib.StorageSystem;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BackupsExtra.Lib.SaveAlgorithmsExtended
{
    public class SplitAlgorithmExtended : SaveAlgorithm
    {
        public SplitAlgorithmExtended(IRepository fileSystem, string backupDir, IZipArchiver archiver, ILogger errorPool) : base(fileSystem, backupDir, archiver, errorPool)
        {
        }

        public override IStorage MakeBackup(IEnumerable<IVirtualObject> objects, string backupName, string restorePointName, IRepository repository)
        {
            var storages = objects.Select(x =>
            {
                string relativePathToArchive = Path.Combine(BackupDir, backupName, restorePointName, x.GetNameWithoutExtension());
                IEnumerable<string> structure = Archiver.Archive(x, relativePathToArchive, Repository);
                BackupObject obj = new BackupObject(x.RelativePath, repository, structure);
                return new ZipStorageExtended(GetType().Name, BackupDir, Repository, new[] {relativePathToArchive }, structure, new List<BackupObject> { obj }, relativePathToArchive);
            });

            return new SplitStorageExtended(storages, GetType().Name, BackupDir, Repository, Path.Combine(BackupDir, backupName, restorePointName));
        }
    }
}

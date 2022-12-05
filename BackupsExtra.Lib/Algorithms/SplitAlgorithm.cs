using Backups.Algorithms.Abstract;
using Backups.ArchiveSystem;
using Backups.BackupSystem;
using Backups.ErrorPool;
using Backups.FileSystem;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Backups.Algorithms
{
    public class SplitAlgorithm : SaveAlgorithm
    {
        public SplitAlgorithm(IRepository fileSystem, string backupDir, IZipArchiver archiver, ILogger errorPool)
            : base(fileSystem, backupDir, archiver, errorPool)
        {
        }

        public override IStorage MakeBackup(IEnumerable<IVirtualObject> objects, string backupTaskName, string restorePointName, IRepository repository)
        {
            IEnumerable<ZipStorage>? storages = objects.Select(x =>
            {
                string relativePathToArchive = Path.Combine(BackupDir, backupTaskName, restorePointName, x.GetNameWithoutExtension());
                IEnumerable<string> structure = Archiver.Archive(x, relativePathToArchive, Repository);
                var obj = new BackupObject(x.RelativePath, repository, structure);
                return new ZipStorage(relativePathToArchive, repository, structure, new List<BackupObject> { obj });
            }).ToList();

            return new SplitStorage(storages);
        }
    }
}

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
    public class SingleAlgorithm : SaveAlgorithm
    {
        public SingleAlgorithm(IRepository fileSystem, string backupDir, IZipArchiver archiver, ILogger errorPool)
            : base(fileSystem, backupDir, archiver, errorPool)
        {
        }

        public override IStorage MakeBackup(IEnumerable<IVirtualObject> objects, string backupTaskName, string restorePointName, IRepository repository)
        {
            string relativePathToArchive = Path.Combine(BackupDir, backupTaskName, restorePointName, "archive_" + restorePointName);

            IEnumerable<BackupObject>? backups = objects.Select(y =>
            {
                return new BackupObject(y.RelativePath, repository, y is IVirtualDirectory dir ? dir.AllSubObjects.Select(x => x.GetRelativeSpecialPath(dir)) : new List<string> { y.Name });
            });

            IEnumerable<string> archiveStructure = Archiver.Archive(objects, relativePathToArchive, Repository);
            return new ZipStorage(relativePathToArchive, Repository, archiveStructure, backups);
        }
    }
}

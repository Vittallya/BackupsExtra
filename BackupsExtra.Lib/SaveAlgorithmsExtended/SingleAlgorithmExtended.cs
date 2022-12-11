using Backups;
using Backups.Algorithms.Abstract;
using Backups.ArchiveSystem;
using Backups.BackupSystem;
using Backups.ErrorPool;
using Backups.FileSystem;
using BackupsExtra.Lib.Extensions;
using BackupsExtra.Lib.StorageSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BackupsExtra.Lib.SaveAlgorithmsExtended
{
    public class SingleAlgorithmExtended : SaveAlgorithm
    {
        public SingleAlgorithmExtended(IRepository fileSystem, string backupDir, IZipArchiver archiver, ILogger errorPool) : base(fileSystem, backupDir, archiver, errorPool)
        {
        }

        public override IStorage MakeBackup(IEnumerable<IVirtualObject> objects, string backupName, string restorePointName, IRepository repository)
        {
            string relativePathToArchive = Path.Combine(BackupDir, backupName, restorePointName, "archive_" + restorePointName);

            var backups = objects.Select(y =>
            {
                return new BackupObject(y.RelativePath, repository, y is IVirtualDirectory dir ?
                    dir.AllSubObjects.Select(x => x.GetRelativeSpecialPath(dir)) : new List<string> { y.Name });
            });

            IEnumerable<string> archiveStructure = Archiver.Archive(objects, relativePathToArchive, Repository);
            return new ZipStorageExtended(GetType().Name, BackupDir, Repository, new[] { relativePathToArchive}, archiveStructure, backups, relativePathToArchive);
        }
    }
}

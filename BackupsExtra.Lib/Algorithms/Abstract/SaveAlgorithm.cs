using Backups.ArchiveSystem;
using Backups.ErrorPool;
using Backups.FileSystem;
using System.Collections.Generic;

namespace Backups.Algorithms.Abstract
{
    public abstract class SaveAlgorithm : ISaveAlgorithm
    {
        private readonly ILogger errorPool;

        public SaveAlgorithm(IRepository fileSystem, string backupDir, IZipArchiver archiver, ILogger errorPool)
        {
            Repository = fileSystem;
            Archiver = archiver;
            this.errorPool = errorPool;
            BackupDir = backupDir;
        }

        public string BackupDir { get; protected set; }

        public IRepository Repository { get; protected set; }
        protected IZipArchiver Archiver { get; }

        public abstract IStorage MakeBackup(IEnumerable<IVirtualObject> objects, string backupTaskName, string restorePointName, IRepository repository);

        public virtual void SetBackupDirectory(string path)
        {
            Repository.CreateDirectory(path);
            BackupDir = path;
        }

        public virtual void SetFileSystem(IRepository fileSystem)
        {
            Repository = fileSystem;
        }
    }
}

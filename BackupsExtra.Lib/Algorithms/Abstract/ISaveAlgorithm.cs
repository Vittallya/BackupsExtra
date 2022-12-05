using Backups.ArchiveSystem;
using Backups.FileSystem;
using System.Collections.Generic;

namespace Backups.Algorithms
{
    public interface ISaveAlgorithm
    {
        public string BackupDir { get; }
        public IRepository Repository { get; }
        public IStorage MakeBackup(IEnumerable<IVirtualObject> objects, string backupTaskName, string restorePointName, IRepository repository);
        public void SetBackupDirectory(string path);
        public void SetFileSystem(IRepository fileSystem);
    }
}

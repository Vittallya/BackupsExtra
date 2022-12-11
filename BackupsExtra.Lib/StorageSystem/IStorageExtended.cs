using Backups.ArchiveSystem;
using Backups.FileSystem;
using System.Collections.Generic;

namespace BackupsExtra.Lib.StorageSystem
{
    public interface IStorageExtended: IStorage
    {
        public string AlgorithmType { get; }

        /// <summary>
        /// Папка хранения стораджа в репозитории
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Общая папка, где хранятся все таски с их точками
        /// </summary>
        public string BackupDir { get; }

        public IRepository StorageRepository { get; }
        public IEnumerable<string> PathToArchives { get; }
    }
}

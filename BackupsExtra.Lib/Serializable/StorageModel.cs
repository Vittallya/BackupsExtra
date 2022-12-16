using System.Collections.Generic;

namespace BackupsExtra.Lib.Serializable
{
    public class StorageModel
    {
        public string AlgorithmType { get; set; }

        /// <summary>
        /// Папка хранения стораджа в репозитории
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Общая папка, где хранятся все таски с их точками
        /// </summary>
        public string BackupDir { get; set; }

        public RepositoryModel StorageRepository { get; set; }
        public IEnumerable<string> PathToArchives { get; set; }
        public IEnumerable<BackupObjectModel> Backups { get; set; }
    }
}

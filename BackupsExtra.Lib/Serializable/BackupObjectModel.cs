using System.Collections.Generic;

namespace BackupsExtra.Lib.Serializable
{
    public class BackupObjectModel
    {
        public string RelativePathToOrig { get; set; }
        public IEnumerable<string> BackupStructure { get; set; }
        public RepositoryModel Repository { get; set; }
    }
}
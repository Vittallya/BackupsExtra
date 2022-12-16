using System;
using System.Collections.Generic;
using System.Text;

namespace BackupsExtra.Lib.Serializable
{
    [Serializable]
    public class BackupTaskModel
    {
        public IEnumerable<RestorePointModel> RestorePoints { get; set; }
        public IEnumerable<string> CurrentTrackingObjects { get; set; }
        public string Name { get; set; }
        public RepositoryModel Repository { get; set; }
    }
}

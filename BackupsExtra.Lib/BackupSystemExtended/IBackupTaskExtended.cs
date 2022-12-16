using Backups.Algorithms;
using Backups.BackupSystem;
using Backups.ErrorPool;
using Backups.FileSystem;
using BackupsExtra.Lib.CleanAlgorithm;
using BackupsExtra.Lib.MergeSystem;
using BackupsExtra.Lib.SaveAlgorithmsExtended;
using BackupsExtra.Lib.SerializeSystem;
using System.Collections.Generic;

namespace BackupsExtra.Lib.BackupSystemExtended
{
    public interface IBackupTaskExtended
    {
        public void Accept(ISerializeVisitor visitor);
        public IEnumerable<RestorePoint> RestorePoints { get; }
        public IEnumerable<string> CurrentTrackingObjects { get; }
        public string Name { get; }
        void ClearTrackingForObject(IVirtualObject obj);
        void StartTrackingForObject(IVirtualObject obj);
        void MakeBackup(SaveAlgorithmExtended saveAlgorithm, IRepository repository, ILogger logger);
        bool RollbackToPoint(int pointNum);
        void Clean(ICleanAlgorithm cleanAlgorithm, IMerger merger, ILogger logger);
    }
}

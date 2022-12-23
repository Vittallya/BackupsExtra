using Backups.Algorithms;
using Backups.BackupSystem;
using Backups.ErrorPool;
using Backups.FileSystem;
using BackupsExtra.Lib.CleanAlgorithm;
using BackupsExtra.Lib.MergeSystem;
using BackupsExtra.Lib.SaveAlgorithmsExtended;
using BackupsExtra.Lib.SerializeSystem;
using System;
using System.Collections.Generic;

namespace BackupsExtra.Lib.BackupSystemExtended
{
    public interface IBackupTaskExtended
    {
        public void Accept(ISerializeVisitor visitor);
        public IRepository Repository { get; }
        public IEnumerable<RestorePoint> RestorePoints { get; }
        public IEnumerable<string> CurrentTrackingObjects { get; }
        public string Name { get; }
        void ClearTrackingForObject(Func<IRepository, IVirtualObject> getter);
        void StartTrackingForObject(Func<IRepository, IVirtualObject> getter);
        void MakeBackup(SaveAlgorithmExtended saveAlgorithm, ILogger logger);
        bool RollbackToPoint(RestorePoint rp, IRepository repo = null);
        void Clean(ICleanAlgorithm cleanAlgorithm, IMerger merger, ILogger logger);
    }
}

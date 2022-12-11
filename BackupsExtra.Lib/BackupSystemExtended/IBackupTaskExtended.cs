using Backups.Algorithms;
using Backups.BackupSystem;
using Backups.FileSystem;
using BackupsExtra.Lib.CleanAlgorithm;
using System.Collections.Generic;

namespace BackupsExtra.Lib.BackupSystemExtended
{
    public interface IBackupTaskExtended
    {
        public IEnumerable<RestorePoint> RestorePoints { get; }
        public string Name { get; }
        void ChangeBackupDirectory(string newDir);
        void ClearTrackingForObject(IVirtualObject obj);
        void StartTrackingForObject(IVirtualObject obj);
        void MakeBackup();
        bool RollbackToPoint(int pointNum);
        void SetupCleanAlgorithm(ICleanAlgorithm cleanAlgorithm);
        void SetupSaveAlgorithm(ISaveAlgorithm algorithm);
        void Clean();
    }
}

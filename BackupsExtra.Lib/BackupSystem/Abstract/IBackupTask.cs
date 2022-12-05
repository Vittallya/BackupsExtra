using Backups.Algorithms;

namespace Backups.BackupSystem
{
    public interface IBackupTask
    {
        public int RestorePointsCount { get; }

        public void Execute();
        public void StartTrackingForObject(string path);
        public void ClearTrackingForObject(string path);
        public void ChangeBackupDirectory(string newDir);
        public void ChangeSaveAlgorithm(ISaveAlgorithm algorithm);

        public bool RollbackToPoint(int pointNum);
    }
}

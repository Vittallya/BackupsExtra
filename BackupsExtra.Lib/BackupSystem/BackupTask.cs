using Backups.Algorithms;
using Backups.FileSystem;
using System;
using System.Collections.Generic;

namespace Backups.BackupSystem
{
    public class BackupTask : IBackupTask
    {
        private readonly IRepository repository;
        private readonly List<RestorePoint> restorePoints;
        private readonly List<IVirtualObject> currentTrackingObjects;
        private ISaveAlgorithm saveAlgorithm;
        private string taskName;
        public BackupTask(ISaveAlgorithm saveAlgorithm, string taskName, IRepository repository)
        {
            this.saveAlgorithm = saveAlgorithm;
            this.taskName = taskName;
            this.repository = repository;
            restorePoints = new List<RestorePoint>();
            currentTrackingObjects = new List<IVirtualObject>();
        }

        public int RestorePointsCount => restorePoints.Count;
        public IReadOnlyList<RestorePoint> RestorePoints => restorePoints;

        public void StartTrackingForObject(string path)
        {
            IVirtualObject obj = repository.GetObject(path);
            currentTrackingObjects.Add(obj);
        }

        public void StartTrackingForObject(IVirtualObject obj)
        {
            if (!currentTrackingObjects.Contains(obj))
                currentTrackingObjects.Add(obj);
        }

        public void Execute()
        {
            ArchiveSystem.IStorage storage = saveAlgorithm.MakeBackup(currentTrackingObjects, taskName, (RestorePointsCount + 1).ToString(), repository);
            var rp = new RestorePoint(RestorePointsCount + 1, DateTime.Now, storage);
            restorePoints.Add(rp);
        }

        public void ChangeBackupDirectory(string newDir)
        {
            saveAlgorithm.SetBackupDirectory(newDir);
        }

        public void ChangeSaveAlgorithm(ISaveAlgorithm algorithm)
        {
            saveAlgorithm = algorithm;
        }

        public bool RollbackToPoint(int pointNum)
        {
            throw new NotImplementedException();
        }

        public void ClearTrackingForObject(string path)
        {
            currentTrackingObjects.RemoveAll(x => x.RelativePath == path);
        }

        public void ClearTrackingForObject(IVirtualObject obj)
        {
            currentTrackingObjects.Remove(obj);
        }
    }
}

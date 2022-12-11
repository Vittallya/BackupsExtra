using Backups.Algorithms;
using Backups.BackupSystem;
using Backups.Comparers;
using Backups.FileSystem;
using BackupsExtra.Lib.CleanAlgorithm;
using BackupsExtra.Lib.MergeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackupsExtra.Lib.BackupSystemExtended
{
    public class BackupTaskExtended : IBackupTaskExtended
    {
        public IEnumerable<RestorePoint> RestorePoints => restorePoints;

        private readonly List<IVirtualObject> currentTrackingObjects;
        private readonly string taskName;
        private List<RestorePoint> restorePoints;
        private ISaveAlgorithm saveAlgorithm;
        private IRepository repository;
        private ICleanAlgorithm cleanAlgorithm;
        private IMerger merger;

        public BackupTaskExtended(string taskName,
                                  ISaveAlgorithm saveAlgorithm,
                                  IRepository repository,
                                  ICleanAlgorithm cleanAlgorithm, IMerger merger)
        {
            this.taskName = taskName;
            this.saveAlgorithm = saveAlgorithm;
            this.repository = repository;
            this.cleanAlgorithm = cleanAlgorithm;
            this.merger = merger;
            currentTrackingObjects = new List<IVirtualObject>();
            restorePoints = new List<RestorePoint>();
        }

        public string Name => throw new NotImplementedException();

        public void ChangeBackupDirectory(string newDir)
        {
            saveAlgorithm.SetBackupDirectory(newDir);
        }

        public void Clean()
        {
            IEnumerable<RestorePoint> pointsToRemove = cleanAlgorithm.
                Clean(restorePoints).
                OrderBy(x => x.Number);

            if (pointsToRemove.Any())
            {
                IEnumerable<RestorePoint> pointsToSave = restorePoints.
                        Except(pointsToRemove, new GenericComparer<RestorePoint>(x => x.Number)).
                        OrderBy(x => x.Number);

                RestorePoint lastSavePoint = pointsToSave.First();
                RestorePoint firstToRemove = pointsToRemove.First();

                List<RestorePoint> list = pointsToRemove.Skip(1).ToList();
                list.Add(lastSavePoint);

                list.ForEach(p =>
                {
                    merger.Merge(firstToRemove, p);
                    firstToRemove = p;
                });

                restorePoints = pointsToSave.ToList();
            }
        }

        public void ClearTrackingForObject(IVirtualObject obj)
        {
            currentTrackingObjects.Remove(obj);
        }

        public void MakeBackup()
        {
            throw new NotImplementedException();
        }

        public bool RollbackToPoint(int pointNum)
        {
            throw new NotImplementedException();
        }

        public void SetupCleanAlgorithm(ICleanAlgorithm cleanAlgorithm)
        {
            this.cleanAlgorithm = cleanAlgorithm;
        }

        public void SetupSaveAlgorithm(ISaveAlgorithm algorithm)
        {
            saveAlgorithm = algorithm;
        }

        public void StartTrackingForObject(IVirtualObject obj)
        {
            currentTrackingObjects.Add(obj);
        }
    }
}

using Backups.Algorithms;
using Backups.BackupSystem;
using Backups.Comparers;
using Backups.ErrorPool;
using Backups.FileSystem;
using BackupsExtra.Lib.CleanAlgorithm;
using BackupsExtra.Lib.MergeSystem;
using BackupsExtra.Lib.SaveAlgorithmsExtended;
using BackupsExtra.Lib.SerializeSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BackupsExtra.Lib.BackupSystemExtended
{
    public class BackupTaskExtended : IBackupTaskExtended
    {
        public IEnumerable<RestorePoint> RestorePoints => restorePoints;
        public string Name => taskName;
        public IEnumerable<string> CurrentTrackingObjects => currentTrackingObjects.Select(x => x.RelativePath);

        private readonly List<IVirtualObject> currentTrackingObjects;
        private readonly string taskName;
        private List<RestorePoint> restorePoints;


        public BackupTaskExtended(string taskName)
        {
            this.taskName = taskName;
            currentTrackingObjects = new List<IVirtualObject>();
            restorePoints = new List<RestorePoint>();
        }

        public BackupTaskExtended(string taskName,
                                  IEnumerable<IVirtualObject> currentTracking,
                                  IEnumerable<RestorePoint> restorePoints): this(taskName)
        {
            currentTrackingObjects = currentTracking.ToList();
            this.restorePoints = restorePoints.ToList();
        }

        public void Clean(ICleanAlgorithm cleanAlgorithm, IMerger merger, ILogger logger)
        {
            IEnumerable<RestorePoint> pointsToRemove = cleanAlgorithm.
                GetPointsToClean(restorePoints).
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

                int count = pointsToRemove.Count();

                restorePoints = pointsToSave.ToList();
                logger.LogMessage($"Back task '{taskName}' perfomed cleaning; ${count} restore points was cleaned");
            }
            else
            {
                logger.LogWarning($"For backup task '{taskName}' was attemt to clean; no restore points to clean was found");
            }
        }

        public void ClearTrackingForObject(IVirtualObject obj)
        {
            currentTrackingObjects.Remove(obj);
        }

        public void MakeBackup(SaveAlgorithmExtended saveAlgorithm, IRepository repository, ILogger logger)
        {
            int count = restorePoints.Count;
            try
            {
                var storage = saveAlgorithm.MakeBackup(currentTrackingObjects, taskName, (count + 1).ToString(), repository);
                var rp = new RestorePoint(count + 1, DateTime.Now, storage);
                restorePoints.Add(rp);
                logger.LogMessage($"For backup task '{taskName}' created new backup with number {count + 1}");
            }
            catch(Exception e)
            {
                logger.AddError(e);
                logger.LogError(e.Message);
            }
        }

        public bool RollbackToPoint(int pointNum)
        {
            throw new NotImplementedException();
        }

        public void StartTrackingForObject(IVirtualObject obj)
        {
            currentTrackingObjects.Add(obj);
        }

        public void Accept(ISerializeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

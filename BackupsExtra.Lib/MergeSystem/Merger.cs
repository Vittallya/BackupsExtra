using Backups;
using Backups.ArchiveSystem;
using Backups.BackupSystem;
using Backups.Comparers;
using Backups.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BackupsExtra.Lib.MergeSystem
{
    class Merger : IMerger
    {
        private readonly IZipArchiver archiver;

        public Merger(IZipArchiver archiver)
        {
            this.archiver = archiver;
        }

        public void Merge(RestorePoint p1, RestorePoint p2)
        {
            IEnumerable<BackupObject> except = p1.Storage.Backups.
                Except(p2.Storage.Backups, new GenericComparer<BackupObject>(x => x.RelativePathToOrig));


            IEnumerable<IVirtualObject> exceptObjects = except.SelectMany(b => p1.Storage.GetObjects(b));
            IEnumerable<IVirtualObject> union = exceptObjects.Union(p2.Storage.GetObjects());

            //todo запись в новый архив (-архивы)
        }
    }
}

using Backups.ArchiveSystem;
using System;

namespace Backups.BackupSystem
{
    public class RestorePoint
    {
        public RestorePoint(int number, DateTime dateTime, IStorage storage)
        {
            Number = number;
            DateTime = dateTime;
            Storage = storage;
        }

        public int Number { get; }
        public DateTime DateTime { get; }
        public IStorage Storage { get; }
    }
}

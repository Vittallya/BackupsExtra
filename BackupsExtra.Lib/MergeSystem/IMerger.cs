using Backups.BackupSystem;

namespace BackupsExtra.Lib.MergeSystem
{
    public interface IMerger
    {
        void Merge(RestorePoint p1, RestorePoint p2);
    }
}

using BackupsExtra.Lib.BackupSystemExtended;

namespace BackupsExtra.Lib.SerializeSystem
{
    public interface ISerializeVisitor
    {
        public void Visit(IBackupTaskExtended backupTask);
    }
}

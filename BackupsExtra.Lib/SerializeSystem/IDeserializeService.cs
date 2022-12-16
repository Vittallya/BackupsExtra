using BackupsExtra.Lib.BackupSystemExtended;

namespace BackupsExtra.Lib.SerializeSystem
{
    public interface IDeserializeService
    {
        public IBackupTaskExtended Deserialize(string json);
    }
}

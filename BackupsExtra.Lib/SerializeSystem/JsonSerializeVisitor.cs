using AutoMapper;
using Backups.ArchiveSystem;
using Backups.BackupSystem;
using Backups.FileSystem;
using BackupsExtra.Lib.BackupSystemExtended;
using BackupsExtra.Lib.MapperProfiles;
using BackupsExtra.Lib.Serializable;
using BackupsExtra.Lib.StorageSystem;
using Newtonsoft.Json;
using System;

namespace BackupsExtra.Lib.SerializeSystem
{
    public class JsonSerializeVisitor : ISerializeVisitor
    {
        private readonly Mapper map;
        private readonly Action<string> saveFunc;

        public JsonSerializeVisitor(Action<string> saveFunc)
        {
            this.map = new Mapper(new MapperConfiguration(c => c.AddProfile(new MainMapperProfile())));
            this.saveFunc = saveFunc;
        }

        public void Visit(IBackupTaskExtended backupTask)
        {
            BackupTaskModel model = map.Map<IBackupTaskExtended, BackupTaskModel>(backupTask);
            string json = JsonConvert.SerializeObject(model);
            saveFunc?.Invoke(json);
        }
    }
}

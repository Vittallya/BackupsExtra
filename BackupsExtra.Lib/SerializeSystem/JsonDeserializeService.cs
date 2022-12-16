using AutoMapper;
using BackupsExtra.Lib.BackupSystemExtended;
using BackupsExtra.Lib.MapperProfiles;
using BackupsExtra.Lib.Serializable;
using Newtonsoft.Json;

namespace BackupsExtra.Lib.SerializeSystem
{
    public class JsonDeserializeService : IDeserializeService
    {
        private readonly Mapper map;
        public JsonDeserializeService()
        {
            this.map = new Mapper(new MapperConfiguration(c => c.AddProfile(new MainMapperProfile())));
        }

        public IBackupTaskExtended Deserialize(string json)
        {
            BackupTaskModel model = JsonConvert.DeserializeObject<BackupTaskModel>(json);
            IBackupTaskExtended task = map.Map<BackupTaskModel, IBackupTaskExtended>(model);
            return task;
        }
    }
}

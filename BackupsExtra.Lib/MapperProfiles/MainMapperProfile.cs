using AutoMapper;
using Backups.ArchiveSystem;
using Backups.BackupSystem;
using Backups.FileSystem;
using BackupsExtra.Lib.BackupSystemExtended;
using BackupsExtra.Lib.Serializable;
using BackupsExtra.Lib.StorageSystem;
using System;

namespace BackupsExtra.Lib.MapperProfiles
{
    public class MainMapperProfile: Profile
    {
        public MainMapperProfile()
        {
            this.CreateMap<IBackupTaskExtended, BackupTaskModel>().ReverseMap();
            this.CreateMap<RestorePoint, RestorePointModel>().ReverseMap();
            this.CreateMap<IStorageExtended, StorageModel>().ReverseMap();
            this.CreateMap<IStorage, StorageModel>(MemberList.Destination).AfterMap((x, y, z) =>
            {
                var extended = (IStorageExtended)x;
                z.Mapper.Map(extended, y);
            }).ReverseMap();


            CreateMap<IRepository, RepositoryModel>(MemberList.Source).AfterMap((x, y, z) =>
            {
                y.RepositoryType = x.GetType().FullName;
            }).
            ReverseMap().
            ConvertUsing(x => (IRepository)Activator.CreateInstance(Type.GetType(x.RepositoryType), new[] { x.AbsoluteRootPath }));

            CreateMap<BackupObject, BackupObjectModel>().ReverseMap();
        }
    }
}

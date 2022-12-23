using AutoMapper;
using Backups.ArchiveSystem;
using Backups.BackupSystem;
using Backups.FileSystem;
using BackupsExtra.Lib.BackupSystemExtended;
using BackupsExtra.Lib.Serializable;
using BackupsExtra.Lib.StorageSystem;
using System;
using System.Linq;

namespace BackupsExtra.Lib.MapperProfiles
{
    public class MainMapperProfile: Profile
    {
        public MainMapperProfile()
        {
            this.CreateMap<IBackupTaskExtended, BackupTaskModel>().ReverseMap().ConvertUsing((model, bt, context) =>
            {
                var filesRepo = context.Mapper.Map<RepositoryModel, IRepository>(model.Repository);

                var objects = model.CurrentTrackingObjects.
                        Where(x => filesRepo.IsDirectoryExists(x) || filesRepo.IsFileExists(x)).
                        Select(x => filesRepo.GetObject(x));

                var rp = model.RestorePoints.Select(x => context.Mapper.Map<RestorePointModel, RestorePoint>(x));

                return new BackupTaskExtended(model.Name, filesRepo, objects, rp);
            });
            this.CreateMap<RestorePoint, RestorePointModel>().ReverseMap().ConvertUsing((model, rp, context) =>
            {
                return new RestorePoint(model.Number, model.DateTime, context.Mapper.Map<StorageModel, IStorage>(model.Storage));
            });

            this.CreateMap<IStorageExtended, StorageModel>().AfterMap((x,y,z) =>
            {
                var extended = (IStorageExtended)x;
                //z.Mapper.Map(extended, y);
                y.StorageType = extended.GetType().FullName;
                if (extended is SplitStorageExtended split)
                    y.InnerZipStorages = split.
                    Storages.
                    ToList().
                    Select(x => z.Mapper.Map<IStorage, StorageModel>(x));
                else if (extended is ZipStorageExtended zip)
                    y.ZipStorageStructure = zip.Structure;
            }).
            ReverseMap();


            this.CreateMap<IStorage, StorageModel>().
            ReverseMap().
            ConvertUsing((storageModel, st, context) =>
            {
                if(storageModel.StorageType == typeof(ZipStorageExtended).FullName)
                {
                    return new ZipStorageExtended(
                        storageModel.AlgorithmType,
                        storageModel.BackupDir,
                        context.Mapper.Map<RepositoryModel, IRepository>(storageModel.StorageRepository),
                        storageModel.PathToArchives,
                        storageModel.ZipStorageStructure,
                        storageModel.Backups.Select(x => context.Mapper.Map<BackupObjectModel, BackupObject>(x)),
                        storageModel.Path);

                }

                else if (storageModel.StorageType == typeof(SplitStorageExtended).FullName)
                {
                    return new SplitStorageExtended(
                        storageModel.InnerZipStorages.Select(x => (ZipStorageExtended)context.Mapper.Map<StorageModel, IStorageExtended>(x)),
                        storageModel.AlgorithmType,
                        storageModel.BackupDir,
                        context.Mapper.Map<RepositoryModel, IRepository>(storageModel.StorageRepository),
                        storageModel.Path);
                }

                throw new ArgumentException("incorrect storage type in configuration");
            });


            CreateMap<IRepository, RepositoryModel>(MemberList.Source).AfterMap((x, y, z) =>
            {
                y.RepositoryType = x.GetType().FullName;
            }).
            ReverseMap().
            ConvertUsing((model, repo, context) => 
            {

                var rep = new HardDriveRepository(model.AbsoluteRootPath, true);
                return (IRepository)rep;
            });

            CreateMap<BackupObject, BackupObjectModel>().ReverseMap().ConvertUsing((model, bObj, context) =>
            {
                return new BackupObject(
                    model.RelativePathToOrig,
                    context.Mapper.Map<RepositoryModel, IRepository>(model.Repository),
                    model.BackupStructure);

            });
        }
    }
}

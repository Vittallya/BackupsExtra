using Backups.BackupSystem;
using Backups.FileSystem;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace BackupsExtra.Lib.StorageSystem
{
    public class ZipStorageExtended : IStorageExtended
    {
        public ZipStorageExtended(  string algorithmType,
                                    string backupDir,
                                    IRepository storageRepository,
                                    IEnumerable<string> pathToArchives,
                                    IEnumerable<string> structure,
                                    IEnumerable<BackupObject> backups,
                                    string rootPath)
        {
            AlgorithmType = algorithmType;
            BackupDir = backupDir;
            StorageRepository = storageRepository;
            PathToArchives = pathToArchives;
            Structure = structure;
            Backups = backups;
            Path = rootPath;
        }

        public string AlgorithmType { get; }
        public IRepository StorageRepository { get; }
        public IEnumerable<string> PathToArchives { get; }
        public IEnumerable<string> Structure { get; }
        public IEnumerable<BackupObject> Backups { get; }
        public string Path { get; }
        public string BackupDir { get; }

        public IEnumerable<IVirtualObject> GetObjects(ZipArchive zip, IEnumerable<string> structure, MemoryRepository repository)
        {
            structure.ToList().ForEach(path =>
            {
                ZipArchiveEntry entry = zip.GetEntry(path);

                if (path.LastIndexOf('\\') == path.Length - 1)
                {
                    repository.CreateDirectory(path);
                }
                else
                {
                    using Stream stream = entry.Open();
                    StreamReader sr = new StreamReader(stream);
                    byte[] arr = sr.CurrentEncoding.GetBytes(sr.ReadToEnd());
                    repository.WriteFile(path, arr);
                }

            });

            IVirtualDirectory obj = (IVirtualDirectory)repository.GetObject("");
            return obj.AllSubObjects;
        }

        public IEnumerable<IVirtualObject> GetObjects()
        {
            using Stream acrhStream = StorageRepository.OpenRead(Path);
            using ZipArchive zip = new ZipArchive(acrhStream);
            MemoryRepository repository = new MemoryRepository(System.IO.Path.GetFileName(Path));
            return GetObjects(zip, Structure, repository);
        }



        public IEnumerable<IVirtualObject> GetObjects(BackupObject obj)
        {
            IEnumerable<string> strcuture = obj.BackupStructure;
            using Stream acrhStream = StorageRepository.OpenRead(Path);
            using ZipArchive zip = new ZipArchive(acrhStream);
            MemoryRepository repository = new MemoryRepository(System.IO.Path.GetFileName(Path));
            return GetObjects(zip, strcuture, repository);
        }
    }
}

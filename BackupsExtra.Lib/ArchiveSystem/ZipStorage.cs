using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.BackupSystem;
using Backups.FileSystem;

namespace Backups.ArchiveSystem
{
    internal class ZipStorage : IStorage
    {
        private readonly IEnumerable<string> archiveStructure;
        public ZipStorage(
            string relativePathToArchive,
            IRepository repository,
            IEnumerable<string> objects,
            IEnumerable<BackupObject> backupObjects)
        {
            RelativePathToArchive = relativePathToArchive;
            Repository = repository;
            archiveStructure = objects;
            Backups = backupObjects;
        }

        public string RelativePathToArchive { get; }
        public IRepository Repository { get; }
        public IEnumerable<BackupObject> Backups { get; }

        public IEnumerable<IVirtualObject> GetObjects()
        {
            IVirtualDirectory dir = new VirtualDirectoryFactory().Create(Path.GetFileName(RelativePathToArchive));

            using Stream str = Repository.OpenRead(RelativePathToArchive);
            using var zip = new ZipArchive(str, ZipArchiveMode.Read);
            var obj = GetObjects(zip, archiveStructure, dir).ToList();
            return obj;
        }

        public IEnumerable<IVirtualObject> GetObjects(BackupObject obj)
        {
            IEnumerable<string> structure = obj.BackupStructure;
            IVirtualDirectory dir = new VirtualDirectoryFactory().Create(Path.GetFileName(RelativePathToArchive));

            using Stream str = Repository.OpenRead(RelativePathToArchive);
            using var zip = new ZipArchive(str, ZipArchiveMode.Read);
            var orig = GetObjects(zip, structure, dir).ToList();
            return orig;
        }

        private IEnumerable<IVirtualObject> GetObjects(ZipArchive zip, IEnumerable<string> structure, IVirtualDirectory dir)
        {
            return structure.Select(path =>
            {
                if (zip.Entries.Any(x => x.FullName == path))
                {
                    ZipArchiveEntry? entry = zip.GetEntry(path);
                    if (entry == null)
                    {
                        throw new InvalidOperationException("Entry does not exist");
                    }

                    if (path.LastIndexOf('\\') == path.Length - 1)
                        return VirtualDirectory.GetOrCreateFolder(dir, path, new VirtualDirectoryFactory());

                    using Stream entryStream = entry.Open();
                    byte[] data = new byte[10240];
                    using var ms = new MemoryStream(data);
                    entryStream.CopyTo(ms);

                    return (IVirtualObject)VirtualFile.CreateFile(path, dir, () => new MemoryStream(data), new VirtualFileFactory(), new VirtualDirectoryFactory());
                }

                throw new ArgumentException($"entry by path '{path}' was deleted from zip storage");
            });
        }
    }
}

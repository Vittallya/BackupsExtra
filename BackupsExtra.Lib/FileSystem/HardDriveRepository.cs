using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Backups.FileSystem
{
    public class HardDriveRepository : IRepository
    {
        public HardDriveRepository(string rootPath, bool create = true)
        {
            AbsoluteRootPath = rootPath;

            if (!Directory.Exists(rootPath))
            {
                if (!create)
                    throw new ArgumentException("directory by root path does not exists");
                else
                    Directory.CreateDirectory(rootPath);
            }
        }

        public string AbsoluteRootPath { get; }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(GetAbsolutePath(path));
        }

        public void DeleteFile(string pathToFile)
        {
            File.Delete(GetAbsolutePath(pathToFile));
        }

        public IEnumerable<string> EnumerateDirectories(string dir)
        {
            return Directory.EnumerateDirectories(GetAbsolutePath(dir));
        }

        public bool IsDirectoryExists(string dirFolder)
        {
            return Directory.Exists(GetAbsolutePath(dirFolder));
        }

        public bool IsFileExists(string path)
        {
            return File.Exists(GetAbsolutePath(path));
        }

        public byte[] ReadFile(string pathToFile)
        {
            return File.ReadAllBytes(GetAbsolutePath(pathToFile));
        }

        public void RemoveDirectories(IEnumerable<string> enumerable)
        {
            if (enumerable.Any())
            {
                using IEnumerator<string> enumarator = enumerable.GetEnumerator();
                do
                    Directory.Delete(enumarator.Current);
                while (enumarator.MoveNext());
            }
        }

        public void WriteFile(string relativePath, byte[] compressed)
        {
            CheckCreateDirectory(relativePath);
            File.WriteAllBytes(GetAbsolutePath(relativePath), compressed);
        }

        public IVirtualObject GetObject(string relativePath)
        {
            if (IsDirectoryExists(relativePath))
            {
                var dirFactory = new VirtualDirectoryFactory();
                IVirtualDirectory root = dirFactory.Create(AbsoluteRootPath);

                var df = new DirectoryInfo(GetAbsolutePath(relativePath));

                bool isRoot = relativePath.Length == 1 && relativePath[0] == '\\';
                var fs = df.EnumerateFileSystemInfos("*", new EnumerationOptions { RecurseSubdirectories = true }).ToList();

                fs.ForEach(x =>
                {
                    IVirtualObject obj;

                    if (x.Attributes == FileAttributes.Directory)
                    {
                        obj = VirtualDirectory.GetOrCreateFolder(root, x.FullName[root.AbsoluteName.Length..], dirFactory);
                    }
                    else
                    {
                        obj = VirtualFile.CreateFile(
                            x.FullName[root.AbsoluteName.Length..],
                            root,
                            () => File.Open(x.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite),
                            new VirtualFileFactory(),
                            dirFactory);
                    }
                });

                return isRoot ? root : VirtualDirectory.GetOrCreateFolder(root, relativePath, dirFactory);
            }
            else if (IsFileExists(relativePath))
            {
                var info = new FileInfo(GetAbsolutePath(relativePath));
                return new VirtualFile(
                    new VirtualDirectory(info?.DirectoryName ?? throw new ArgumentException("Directory name is null")),
                    info.Name,
                    () => File.Open(info.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite));
            }

            throw new ArgumentException("file or directory was not found");
        }

        public Stream OpenWrite(string path)
        {
            CheckCreateDirectory(path);
            return File.OpenWrite(GetAbsolutePath(path));
        }

        public Stream OpenRead(string relativePath)
        {
            return File.OpenRead(GetAbsolutePath(relativePath));
        }

        public string GetRelativePath(string x)
        {
            return Path.GetRelativePath(AbsoluteRootPath, x);
        }

        internal void CheckCreateDirectory(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(GetAbsolutePath(path))))
                Directory.CreateDirectory(Path.GetDirectoryName(GetAbsolutePath(path ?? throw new ArgumentException("Path is null")) ?? throw new ArgumentException("Absolute path is null")) ?? throw new ArgumentException("Directory is not created"));
        }

        private string GetAbsolutePath(string path)
        {
            return Path.Combine(AbsoluteRootPath, path);
        }
    }
}

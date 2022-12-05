using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Backups.FileSystem
{
    public class MemoryRepository : IRepository
    {
        private readonly VirtualDirectory root;

        public MemoryRepository(string name)
        {
            root = new VirtualDirectory(name);
        }

        public char Separator { get; } = '\\';
        public string AbsoluteRootPath => root.AbsoluteName;
        public void CreateDirectory(string path)
        {
            VirtualDirectory.GetOrCreateFolder(root, path, new VirtualDirectoryFactory());
        }

        public void DeleteFile(string pathToFile)
        {
            if (TryGetFile(root, pathToFile, out IVirtualFile? file, out IVirtualDirectory? dir))
            {
                dir?.RemoveObject(file ?? throw new ArgumentException("File with such path not found"));
            }
        }

        public bool IsDirectoryExists(string dirFolder)
        {
            return TryGetDir(root, dirFolder, out _);
        }

        public bool IsFileExists(string path)
        {
            return TryGetFile(root, path, out _, out _);
        }

        public byte[] ReadFile(string pathToFile)
        {
            if (TryGetFile(root, pathToFile, out IVirtualFile? file, out _))
            {
                using var mem = new MemoryStream();
                using Stream str = file?.GetStream() ?? throw new ArgumentException("Invalid path");
                return mem.ToArray();
            }

            throw new ArgumentException("file was not found");
        }

        public void RemoveDirectories(IEnumerable<string> enumerable)
        {
            foreach (string dirStr in enumerable)
            {
                if (TryGetDir(root, dirStr, out IVirtualDirectory? vd))
                {
                    vd?.Parent.RemoveObject(vd);
                }
            }
        }

        public void WriteFile(string relativePath, byte[] compressed)
        {
            Stream FuncStr() => new MemoryStream(compressed);

            if (TryGetFile(root, relativePath, out IVirtualFile? file, out _))
            {
                file?.SetNewFuncStream(FuncStr);
            }
            else
            {
                VirtualFile.CreateFile(relativePath, root, FuncStr, new VirtualFileFactory(), new VirtualDirectoryFactory());
            }
        }

        public IVirtualObject GetObject(string relativePath)
        {
            if (TryGetDir(root, relativePath + Separator, out IVirtualDirectory? dir))
            {
                return dir ?? throw new ArgumentException("Dir not found");
            }
            else if (TryGetFile(root, relativePath, out IVirtualFile? file, out _))
            {
                return file ?? throw new ArgumentException("File not found");
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public string GetName(string path)
        {
            int index = path.LastIndexOf(Separator);

            if (index == -1)
            {
                return path;
            }
            else if (index == path.Length - 1)
            {
                path = path.TrimEnd(Separator);
            }

            return path[(path.LastIndexOf(Separator) + 1) ..];
        }

        public Stream OpenWrite(string path)
        {
            byte[] arr = new byte[102400];
            var str = new MemoryStream(arr, true);

            if (TryGetFile(root, path, out IVirtualFile? file, out _))
            {
                file?.SetNewFuncStream(() => new MemoryStream(arr));
            }
            else
            {
                VirtualFile.CreateFile(path, root, () => new MemoryStream(arr), new VirtualFileFactory(), new VirtualDirectoryFactory());
            }

            return str;
        }

        public Stream OpenRead(string relativePath)
        {
            if (TryGetFile(root, relativePath, out IVirtualFile? file, out _))
            {
                return file?.GetStream() ?? throw new ArgumentException("Invalid path to file");
            }

            throw new ArgumentException("incorrect path");
        }

        public string GetRelativePath(string x)
        {
            if (x == root.AbsoluteName)
                return x;

            return x[(root.AbsoluteName.Length + 1) ..];
        }

        internal void CheckCreateDirectory(string absolutePath)
        {
            if (!TryGetDir(root, absolutePath, out _))
                CreateDirectory(absolutePath);
        }

        protected bool TryGetFile(IVirtualDirectory dir, string path, out IVirtualFile? file, out IVirtualDirectory? lastDir)
        {
            lastDir = root;
            file = null;

            if (TryGetDir(dir, path, out lastDir))
            {
                file = lastDir?.SubFiles.FirstOrDefault(x => GetName(path) == x.Name);
            }

            return file != null;
        }

        protected bool TryGetDir(IVirtualDirectory dir, string path, out IVirtualDirectory? current)
        {
            if (path.Contains(Separator))
            {
                path = path[..path.LastIndexOf(Separator)];
            }

            var pathParts = new Queue<string>(path.Split(Separator));

            current = dir;

            while (pathParts.TryDequeue(out string? part))
            {
                if (part.Length == 0)
                    continue;

                current = current.SubFolders.FirstOrDefault(x => x.Name == part);
                if (current == null)
                    return false;
            }

            return true;
        }
    }
}

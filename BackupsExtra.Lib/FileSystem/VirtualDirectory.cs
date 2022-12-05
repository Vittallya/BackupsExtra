using Backups.FileSystem.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backups.FileSystem
{
    internal class VirtualDirectory : VirtualObject, IVirtualDirectory
    {
        private readonly List<IVirtualObject> _objects = new List<IVirtualObject>();

        public VirtualDirectory(IVirtualDirectory parent, string name)
            : base(parent, name)
        {
            IsRoot = false;
        }

        public VirtualDirectory(string fullName)
            : base(fullName)
        {
            IsRoot = true;
        }

        public bool IsRoot { get; }

        public IReadOnlyList<IVirtualObject> SubObjects => _objects;
        public IReadOnlyList<IVirtualFile> SubFiles => _objects.OfType<IVirtualFile>().ToList();
        public IReadOnlyList<IVirtualDirectory> SubFolders => _objects.OfType<IVirtualDirectory>().ToList();

        public IReadOnlyList<IVirtualObject> AllSubObjects => GetAllObjects(this, new List<IVirtualObject>());

        public static string[] GetObjects<TDir>(TDir dir)
            where TDir : class, IVirtualDirectory
        {
            return dir.AllSubObjects.Select(x => x.GetAbsoluteSpecialPath()).ToArray();
        }

        public static TDir GetOrCreateFolder<TDir>(TDir dir, string path, IVirtualDirectoryFactory<TDir> factory)
            where TDir : class, IVirtualDirectory
        {
            if (path.Length == 0 || (path.Length == 1 && path[0] == '\\'))
                throw new ArgumentException("Incorrect path");

            var pathParts = new Queue<string>(path.Split('\\'));

            TDir current = null;
            TDir parent = dir;

            while (pathParts.TryDequeue(out string? part))
            {
                if (part.Length == 0)
                    continue;

                current = parent.SubFolders.FirstOrDefault(x => x.Name == part) as TDir;

                if (current == null)
                {
                    current = factory.Create(parent, part);
                    parent.AddObjects(current);
                }

                parent = current;
            }

            return current ?? throw new ArgumentException("Could not create folder");
        }

        public static bool TryGetDir<TDir>(TDir dir, string path, out TDir current)
            where TDir : class, IVirtualDirectory
        {
            var pathParts = new Queue<string>(path.Split('\\'));

            current = dir;

            while (pathParts.TryDequeue(out string? part))
            {
                if (part.Length == 0)
                    continue;

                current = current.SubFolders.FirstOrDefault(x => x.Name == part) as TDir;
                if (current == null)
                    return false;
            }

            return true;
        }

        public override void Accept(IZipVisitor visitor, IVirtualDirectory directParent)
        {
            if (_objects.Any())
                _objects.ForEach(x => x.Accept(visitor, directParent));
            else
                visitor.Visit(this, directParent);
        }

        void IVirtualDirectory.AddObjects(params IVirtualObject[] objects)
        {
            _objects.AddRange(objects);
        }

        void IVirtualDirectory.RemoveObject(IVirtualObject obj)
        {
            _objects.Remove(obj);
        }

        private IReadOnlyList<IVirtualObject> GetAllObjects(IVirtualDirectory dir, List<IVirtualObject> objects)
        {
            IReadOnlyList<IVirtualDirectory> subFolders = dir.SubFolders;
            IEnumerable<IVirtualDirectory> foldersFat = subFolders.Where(x => x.SubObjects.Any());

            foreach (IVirtualDirectory folder in foldersFat)
            {
                objects.AddRange(folder.AllSubObjects);
            }

            objects.AddRange(dir.SubFiles);
            objects.AddRange(subFolders.Except(foldersFat));
            return objects;
        }
    }
}

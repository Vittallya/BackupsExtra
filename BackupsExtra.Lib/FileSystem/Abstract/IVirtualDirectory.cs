using System.Collections.Generic;

namespace Backups.FileSystem
{
    public interface IVirtualDirectory : IVirtualObject
    {
        public bool IsRoot { get; }
        public IReadOnlyList<IVirtualObject> SubObjects { get; }
        public IReadOnlyList<IVirtualObject> AllSubObjects { get; }
        public IReadOnlyList<IVirtualFile> SubFiles { get; }
        public IReadOnlyList<IVirtualDirectory> SubFolders { get; }

        internal void AddObjects(params IVirtualObject[] objects);
        internal void RemoveObject(IVirtualObject obj);
    }
}

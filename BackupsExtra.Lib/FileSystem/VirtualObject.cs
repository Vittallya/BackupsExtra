using System;
using System.Diagnostics.CodeAnalysis;

namespace Backups.FileSystem
{
    public abstract class VirtualObject : IVirtualObject, IEquatable<VirtualObject>
    {
        protected VirtualObject(string fullPath)
        {
            if (fullPath.Contains('\\'))
            {
                Name = fullPath[(fullPath.LastIndexOf('\\') + 1) ..] ?? throw new ArgumentException("Invalid name");
                Path = fullPath[..fullPath.LastIndexOf('\\')] ?? throw new ArgumentException("Invalid path");
            }
            else
            {
                Name = fullPath;
            }

            Parent = (IVirtualDirectory)this;
            Path = string.Empty;
            RootDir = (IVirtualDirectory)this;
            RelativePath = Name;
        }

        protected VirtualObject(IVirtualDirectory parent, string name)
        {
            if (parent is null)
                throw new ArgumentException("parent is null");

            Parent = parent;
            Name = name;
            Path = parent.AbsoluteName;
            RelativePath = System.IO.Path.Combine(parent.RelativePath, Name);

            IVirtualDirectory? curr = parent;

            while (!curr.IsRoot)
                curr = curr.Parent;

            RootDir = curr;

            RelativePath = System.IO.Path.GetRelativePath(curr.AbsoluteName, AbsoluteName);
        }

        public IVirtualDirectory Parent { get; }
        public string Name { get; }

        public string Path { get; }

        public string AbsoluteName => Path != null ? System.IO.Path.Combine(Path, Name) : Name;

        public IVirtualDirectory RootDir { get; }

        public string RelativePath { get; }

        public abstract void Accept(IZipVisitor visitor, IVirtualDirectory directParent);
        public bool Equals([AllowNull] VirtualObject other) => AbsoluteName.Equals(other?.AbsoluteName);

        public override bool Equals(object? obj)
        {
            return obj is VirtualObject vo ? vo.AbsoluteName.Equals(AbsoluteName) : base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return AbsoluteName?.GetHashCode() ?? throw new ArgumentException();
        }
    }
}

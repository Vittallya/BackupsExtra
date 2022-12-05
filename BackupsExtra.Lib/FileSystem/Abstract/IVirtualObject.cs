namespace Backups.FileSystem
{
    public interface IVirtualObject
    {
        public IVirtualDirectory RootDir { get; }
        public string Name { get; }
        public string Path { get; }
        public string RelativePath { get; }
        public string AbsoluteName { get; }
        public IVirtualDirectory Parent { get; }
        void Accept(IZipVisitor visitor, IVirtualDirectory directParent);
    }
}
namespace Backups.FileSystem
{
    public interface IVisitor
    {
        public void Visit(IVirtualFile file);
        public void Visit(IVirtualDirectory file);
        void AddParent(string relativePath);
    }
}

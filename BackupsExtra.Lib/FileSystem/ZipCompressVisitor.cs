using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Backups.FileSystem
{
    public class ZipCompressVisitor : IZipVisitor
    {
        private readonly ZipArchive zip;

        private readonly List<ZipArchiveEntry> entries = new List<ZipArchiveEntry>();

        public ZipCompressVisitor(ZipArchive zip)
        {
            this.zip = zip;
        }

        public IEnumerable<ZipArchiveEntry> Entries => entries;

        public void Visit(IVirtualFile file, IVirtualDirectory directParent)
        {
            ZipArchiveEntry entry = zip.CreateEntry(GetRelativePath(file, directParent), CompressionLevel.Optimal);
            using Stream entryStr = entry.Open();
            using Stream fileStr = file.GetStream();
            fileStr.CopyTo(entryStr);
            entries.Add(entry);
        }

        public void Visit(IVirtualDirectory file, IVirtualDirectory directParent)
        {
            entries.Add(zip.CreateEntry(GetRelativePath(file, directParent) + '\\', CompressionLevel.Optimal));
        }

        private static string GetRelativePath(IVirtualObject obj, IVirtualDirectory directParent)
        {
            return directParent.IsRoot ? obj.RelativePath :
                Path.GetRelativePath(directParent.Parent.RelativePath, obj.RelativePath);
        }
    }
}

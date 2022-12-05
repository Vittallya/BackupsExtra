using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.FileSystem;

namespace Backups.ArchiveSystem
{
    public class ZipArchiver : IZipArchiver
    {
        public IEnumerable<string> Archive(IVirtualObject obj, string path, IRepository rep)
        {
            using Stream archStream = rep.OpenWrite(path);
            using var zipArchive = new ZipArchive(archStream, ZipArchiveMode.Create, true);
            IZipVisitor visitor = new ZipCompressVisitor(zipArchive);
            obj.Accept(visitor, obj.Parent);
            return visitor.Entries.Select(x => x.FullName);
        }

        public IEnumerable<string> Archive(IEnumerable<IVirtualObject> obj, string path, IRepository rep)
        {
            using Stream archStream = rep.OpenWrite(path);
            using var zipArchive = new ZipArchive(archStream, ZipArchiveMode.Create, true);
            IZipVisitor visitor = new ZipCompressVisitor(zipArchive);
            obj.ToList().ForEach(x => x.Accept(visitor, x.Parent));
            return visitor.Entries.Select(x => x.FullName);
        }
    }
}

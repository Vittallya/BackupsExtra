using Backups.ArchiveSystem;
using Backups.FileSystem;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;

namespace BackupsExtra.Lib.ArchiveSystemExtended
{
    public class ZipArchiverExtended : ZipArchiver, IZipArchiverUpdater
    {
        public void Update(IEnumerable<IVirtualObject> objects, string path, IRepository rep)
        {
            using var archStream = rep.OpenWrite(path);
            using ZipArchive zipArchive = new ZipArchive(archStream, ZipArchiveMode.Create, true);
            IZipVisitor visitor = new ZipCompressVisitor(zipArchive);
            obj.ToList().ForEach(x => x.Accept(visitor, x.Parent));
            return visitor.Entries.Select(x => x.FullName);
        }
    }
}

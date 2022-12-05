using System;
using System.IO;

namespace Backups.FileSystem
{
    public interface IVirtualFile : IVirtualObject
    {
        public string NameWithoutExtension { get; }
        public string Extension { get; }
        public Stream GetStream();

        internal void SetNewFuncStream(Func<Stream> func);
    }
}
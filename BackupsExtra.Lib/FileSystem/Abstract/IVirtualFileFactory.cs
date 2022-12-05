using System;
using System.IO;

namespace Backups.FileSystem.Abstract
{
    public interface IVirtualFileFactory<in TDir, out TFile>
      where TDir : class, IVirtualDirectory
      where TFile : class, IVirtualFile
    {
        TFile Create(TDir dir, string name, Func<Stream> func);
    }
}

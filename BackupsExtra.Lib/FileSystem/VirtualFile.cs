using Backups.FileSystem.Abstract;
using System;
using System.IO;
using System.Linq;

namespace Backups.FileSystem
{
    internal class VirtualFile : VirtualObject, IVirtualFile
    {
        private Func<Stream> funcStream;

        public VirtualFile(IVirtualDirectory parent, string name, Func<Stream> funcStream)
            : base(parent, name)
        {
            if (!name.Contains('.'))
            {
                NameWithoutExtension = name;
                Extension = string.Empty;
            }
            else
            {
                NameWithoutExtension = name[..name.LastIndexOf('.')];
                Extension = name[name.LastIndexOf('.') ..];
            }

            this.funcStream = funcStream;
        }

        public string NameWithoutExtension { get; }
        public string Extension { get; }
        public static TFile CreateFile<TDir, TFile>(
            string path,
            TDir root,
            Func<Stream> func,
            IVirtualFileFactory<TDir, TFile> fileFactory,
            IVirtualDirectoryFactory<TDir> folderFactory)

            where TDir : class, IVirtualDirectory
            where TFile : class, IVirtualFile
        {
            TFile file = null;

            if (path.Length == 0 || path[^1] == '\\')
                throw new ArgumentException("Incorrect path to file");

            if (path.Contains('\\'))
            {
                string fileName = path[(path.LastIndexOf('\\') + 1) ..];
                string pathToFile = path[..path.LastIndexOf('\\')];

                if (VirtualDirectory.TryGetDir(root, pathToFile, out TDir dir))
                {
                    if (dir?.SubFiles.Any(x => x.Name == fileName) ?? false)
                        throw new ArgumentException("this file already exists");
                }

                if (dir == null)
                {
                    dir = VirtualDirectory.GetOrCreateFolder(root, pathToFile, folderFactory);
                }

                file = fileFactory.Create(dir, fileName, func);
                dir.AddObjects(file);
            }
            else
            {
                file = fileFactory.Create(root, path, func);
                root.AddObjects(file);
            }

            return file;
        }

        public Stream GetStream() => funcStream();

        void IVirtualFile.SetNewFuncStream(Func<Stream> func)
        {
            funcStream = func;
        }

        public override void Accept(IZipVisitor visitor, IVirtualDirectory directParent)
        {
            visitor.Visit(this, directParent);
        }
    }
}

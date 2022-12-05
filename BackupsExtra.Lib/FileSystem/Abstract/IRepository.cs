using System.Collections.Generic;
using System.IO;

namespace Backups.FileSystem
{
    public interface IRepository
    {
        string AbsoluteRootPath { get; }
        byte[] ReadFile(string pathToFile);
        void WriteFile(string relativePath, byte[] compressed);
        void CreateDirectory(string path);
        void RemoveDirectories(IEnumerable<string> enumerable);
        bool IsFileExists(string path);
        void DeleteFile(string pathToFile);
        bool IsDirectoryExists(string dirFolder);
        Stream OpenWrite(string path);

        IVirtualObject GetObject(string relativePath);
        Stream OpenRead(string relativePath);
        string GetRelativePath(string x);
    }
}
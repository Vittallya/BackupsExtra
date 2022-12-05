using Backups.FileSystem;
using System.Collections.Generic;

namespace Backups.BackupSystem
{
    public class BackupObject
    {
        public BackupObject(string relativePathToFile, IRepository repository, IEnumerable<string> structure)
        {
            RelativePathToOrig = relativePathToFile;
            Repository = repository;
            BackupStructure = structure;
        }

        public string RelativePathToOrig { get; }
        public IEnumerable<string> BackupStructure { get; }
        public IRepository Repository { get; }
    }
}

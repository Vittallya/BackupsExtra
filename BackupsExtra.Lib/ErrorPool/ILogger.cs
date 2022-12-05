using System;

namespace Backups.ErrorPool
{
    public interface ILogger
    {
        void AddError(Exception e);

        void LogError(string err);
        void LogWarning(string msg);
        void LogMessage(string msg);
    }
}

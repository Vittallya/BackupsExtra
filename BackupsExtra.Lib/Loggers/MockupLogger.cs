using Backups.ErrorPool;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackupsExtra.Lib.Loggers
{
    class MockupLogger : ILogger
    {
        public void AddError(Exception e)
        {
            
        }

        public ErrorEntity GetLastError()
        {
            return null;
        }

        public void LogError(string err)
        {
        }

        public void LogMessage(string msg)
        {
        }

        public void LogWarning(string msg)
        {
        }
    }
}

using System;
using System.Collections.Generic;

namespace Backups.ErrorPool
{
    public class ErrorPool : ILogger
    {
        private readonly List<ErrorEntity> errors = new List<ErrorEntity>();

        public void AddError(Exception e)
        {
            errors.Add(new ErrorEntity(e, e.Message));
        }

        public void LogError(string err)
        {
            throw new NotImplementedException();
        }

        public void LogMessage(string msg)
        {
            throw new NotImplementedException();
        }

        public void LogWarning(string msg)
        {
            throw new NotImplementedException();
        }
    }
}

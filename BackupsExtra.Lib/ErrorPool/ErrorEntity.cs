using System;

namespace Backups.ErrorPool
{
    public class ErrorEntity
    {
        public ErrorEntity(Exception exception, string message)
        {
            Exception = exception;
            Message = message;
        }

        public Exception Exception { get; }
        public string Message { get; }
    }
}

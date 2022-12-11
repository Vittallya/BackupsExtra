using Backups.ErrorPool;
using Backups.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BackupsExtra.Lib.Loggers
{
    public class FileLogger : ILogger
    {
        private readonly IRepository repository;
        private readonly string relativePathToFile;
        private readonly LoggerConfiguration config;

        private readonly Queue<Exception> exceptions;

        public FileLogger(IRepository repository, string relativePathToFile, Action<LoggerConfiguration> options)
        {
            this.repository = repository;
            this.relativePathToFile = relativePathToFile;
            config = new LoggerConfiguration();
            options(config);
            exceptions = new Queue<Exception>();
        }


        public void AddError(Exception e)
        {
            exceptions.Enqueue(e);
        }

        public ErrorEntity GetLastError()
        {
            return exceptions.TryPeek(out Exception ex) ? new ErrorEntity(ex, ex.Message) : null;
        }

        public void LogError(string err)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (config.IncludeTimeCode)
                stringBuilder.Append($"[{DateTime.UtcNow.ToString("HH:mm:ss:ff")}];");
            stringBuilder.Append("!ERROR!;");
            stringBuilder.Append(err);
            LogSomething(stringBuilder.ToString());
        }
        
        public void LogSomething(string message)
        {
            using Stream configFileStream = repository.OpenWrite(relativePathToFile);
            using StreamWriter writer = new StreamWriter(configFileStream);
            writer.WriteLine(message);
        }

        public void LogMessage(string msg)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (config.IncludeTimeCode)
                stringBuilder.Append($"[{DateTime.UtcNow.ToString("HH:mm:ss:ff")}];");
            stringBuilder.Append("Message;");
            stringBuilder.Append(msg);
            LogSomething(stringBuilder.ToString());
        }

        public void LogWarning(string msg)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (config.IncludeTimeCode)
                stringBuilder.Append($"[{DateTime.UtcNow.ToString("HH:mm:ss:ff")}];");
            stringBuilder.Append("!WARNING!;");
            stringBuilder.Append(msg);
            LogSomething(stringBuilder.ToString());
        }
    }
}

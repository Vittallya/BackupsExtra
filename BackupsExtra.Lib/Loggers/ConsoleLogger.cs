using Backups.ErrorPool;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackupsExtra.Lib.Loggers
{
    public class ConsoleLogger: ILogger
    {
        private readonly LoggerConfiguration config;

        private readonly Queue<Exception> exceptions;

        public ConsoleLogger(Action<LoggerConfiguration> options)
        {
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
            Console.WriteLine(message);
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

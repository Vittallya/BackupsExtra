namespace BackupsExtra.Lib.Loggers
{
    public class LoggerConfiguration
    {
        public bool IncludeTimeCode { get; private set; }

        public void AddTimeCode()
        {
            IncludeTimeCode = true;
        }
    }
}

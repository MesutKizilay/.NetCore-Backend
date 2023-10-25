namespace Core.CrossCuttingConcerns.Logging.Log4Net.Loggers
{
    public class FileLogger : Log4NetServiceBase
    {
        public FileLogger() : base("JsonFileLogger")
        {
        }
    }
}
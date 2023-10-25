using Core.CrossCuttingConcerns.Logging.SeriLog.ConfigurationModels;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Logging.SeriLog.Loggers
{
    public class FileLogger : SeriLogServiceBase
    {
        private readonly IConfiguration _configuration;

        public FileLogger()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build(); //SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory) // Uygulamanın çalıştığı dizini ayarlayın.

            FileLogConfiguration logConfig = _configuration.GetSection("SeriLogConfigurations:FileLogConfiguration").Get<FileLogConfiguration>();

            string logFilePath = string.Format(format: "{0}{1}", arg0: Directory.GetCurrentDirectory() + logConfig.FolderPath, arg1: ".txt");

            Logger = new LoggerConfiguration().WriteTo.File(path: logFilePath,
                                                            rollingInterval: RollingInterval.Day,
                                                            retainedFileCountLimit: null,
                                                            fileSizeLimitBytes: 5000000,
                                                            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}")
                                                            .CreateLogger();
        }
    }
}
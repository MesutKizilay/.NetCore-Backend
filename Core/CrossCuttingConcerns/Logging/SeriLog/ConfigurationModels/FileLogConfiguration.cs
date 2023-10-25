using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Core.CrossCuttingConcerns.SeriLog.ConfigurationModels
namespace Core.CrossCuttingConcerns.Logging.SeriLog.ConfigurationModels
{
    public class FileLogConfiguration
    {
        public string FolderPath { get; set; }

        public FileLogConfiguration()
        {
            FolderPath = string.Empty;
        }

        public FileLogConfiguration(string folderPath)
        {
            FolderPath = folderPath;
        }
    }
}
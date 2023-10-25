using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Logging.Log4Net.Loggers
{
    public class DatabaseLogger : Log4NetServiceBase
    {
        public DatabaseLogger() : base("DatabaseLogger")
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Lib.Log
{
    public static class SHLog4net
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger("");
        public static void LogInfo(string message)
        {
            log.Info(message);
        }
        public static void LogError(Exception ex)
        {
            log.Error(ex);
        }
    }
}

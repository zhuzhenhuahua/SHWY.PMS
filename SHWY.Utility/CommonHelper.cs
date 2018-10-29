using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Utility
{
   public static class CommonHelper
    {
        public static string GetRandomString(string startWith)
        {
            Random r = new Random();
            return startWith + DateTime.Now.ToString("yyyyMMddHHmmss")+ r.Next(1000, 9999).ToString();
        }
        #region 本机信息
        public static string GetHostName()
        {
            return Environment.MachineName;
        }

        public static string GetLoggerName(Type t)
        {
            return t.ToString();
        }

        public static string GetThreadId()
        {
            return System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
        }

        public static string GetOSName()
        {
            return Environment.OSVersion.ToString();
        }

        public static string GetDomain()
        {
            return AppDomain.CurrentDomain.FriendlyName;
        }
        #endregion
    }
}

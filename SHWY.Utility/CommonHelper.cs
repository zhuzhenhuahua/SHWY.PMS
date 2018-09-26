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
    }
}

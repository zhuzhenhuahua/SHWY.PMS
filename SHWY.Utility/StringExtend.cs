using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Utility
{
    public static class StringExtend
    {
        public static string SubStr(this string str, int length)
        {
            if (str.Length > length)
                return str.Substring(0, length)+"...";
            return str;
        }
    }
}

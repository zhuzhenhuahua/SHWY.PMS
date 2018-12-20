using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Lib.Util
{
   public static class StringJoinHelper
    {
        /// <summary>
        /// 获取空占位符
        /// </summary>
        /// <param name="len">占位符个数</param>
        public static string GetNBSPLen(int len)
        {
            StringBuilder sbNbsp = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                sbNbsp.Append("&nbsp;");
            }
            return sbNbsp.ToString();
        }
    }
}

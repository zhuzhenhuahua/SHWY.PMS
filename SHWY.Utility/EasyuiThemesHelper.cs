using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Utility
{
    public static class EasyuiThemesHelper
    {
        private const string DefaultThemes = "default";
        private static ConcurrentDictionary<int, string> dicEasyuiThemes = new ConcurrentDictionary<int, string>();
        public static bool ContainsKey(int key)
        {
            return dicEasyuiThemes.ContainsKey(key);
        }
        public static void SetValue(int key, string value)
        {
            dicEasyuiThemes[key] = value;
        }
        public static string GetValue(int key)
        {
            if (ContainsKey(key))
                return dicEasyuiThemes[key];
            return DefaultThemes;
        }
    }
}

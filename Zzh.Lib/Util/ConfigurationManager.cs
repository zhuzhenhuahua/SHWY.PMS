using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zzh.Lib.Util
{
    public static class ConfigurationManager
    {
        static ConcurrentDictionary<Type, Object> _dic;
        static object _syncObject = new object();
        public static T GetConfig<T>() where T : class, new()
        {
            try
            {
                Type t = typeof(T);
                if (_dic.ContainsKey(t))
                    return _dic[t] as T;
                lock (_syncObject)
                {
                    if (_dic.ContainsKey(t))
                        return _dic[t] as T;
                    return (T)GetConfigByType(t);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static object GetConfigByType(Type t)
        {
            if (_dic.ContainsKey(t))
                return _dic[t];
            return null;
        }
    }
}

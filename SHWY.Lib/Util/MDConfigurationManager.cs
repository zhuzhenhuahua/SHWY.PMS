using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Lib.DB.Context;
using SHWY.Model.DB;
using SHWY.Model.DB.Configuration;
using SHWY.Model.DB.Configuration.Att;

namespace SHWY.Lib.Util
{
    public static class MDConfigurationManager
    {
        static string _appDomain;
        static ConcurrentDictionary<Type, Object> _dic;
        static ProContext _db;
        static object _syncObject = new object();
        static MDConfigurationManager()
        {
            lock (_syncObject)
            {
                Init();
            }
        }
        static void Init()
        {
            _dic = new ConcurrentDictionary<Type, object>();
            string s = System.Configuration.ConfigurationManager.AppSettings["ConfigConn"];
            if (!string.IsNullOrEmpty(s))
                _db = new ProContext(s);
            else
                _db = new ProContext();
            _appDomain = System.Configuration.ConfigurationManager.AppSettings["ConfigDomain"];
        }
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

            object[] bkatts = t.GetCustomAttributes(typeof(MDConfigAttribute), false);
            if (bkatts == null || bkatts.Length == 0)
                return null;
            MDConfigAttribute att = bkatts[0] as MDConfigAttribute;
            Sys_Configuration item = new Sys_Configuration();
            item.Module = att.Module; item.Functions = att.Functions;
            var query = _db.SysConfigurations.Where(c => c.Domain == _appDomain).Where(a => a.Module == item.Module).Where(b => b.Functions == item.Functions).ToList();
            if (query != null && query.Count > 0)
            {
                object ret = t.Assembly.CreateInstance(t.ToString());
                foreach (var i in query)
                {
                    var property = ret.GetType().GetProperty(i.Keys);
                    if (property != null)
                        property.SetValue(ret, i.Value);
                }
                _dic[t] = ret;
                return ret;
            }
            return null;
        }
    }
}

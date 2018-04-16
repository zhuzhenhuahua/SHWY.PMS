using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zzh.Model.DB.Configuration.Att
{
   public class MDConfigAttribute:Attribute
    {
        string _moduleName, _functionName;
        public MDConfigAttribute(string module,string func)
        {
            _moduleName = module;
            _functionName = func;
        }
        public MDConfigAttribute(string module)
        {
            _moduleName = module;
        }
        public string Module
        {
            get { return _moduleName; }
            set { _moduleName = value; }
        }
        public string Functions
        {
            get { return _functionName; }
            set { _functionName = value; }
        }
    }
    public class MDKeyAttribute : Attribute
    {
        public string Key { get; set; }
        public MDKeyAttribute(string key)
        {
            Key = key;
        }
    }
}

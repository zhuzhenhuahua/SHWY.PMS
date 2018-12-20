using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB;

namespace SHWY.Model.Join
{
    /// <summary>
    /// 项目产品信息汇总
    /// </summary>
    [Serializable]
    public class ItemProdServer
    {
        /// <summary>
        /// 通讯协议
        /// </summary>
        public string porttypeName { get; set; }
        public Product prod { get; set; }

        public Servers server { get; set; }

    }
}

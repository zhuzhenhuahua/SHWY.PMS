using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zzh.Model.DB
{
    [Serializable]
    [Table("App_Asn_Head")]
    public class EntAppAsnHead
    {
        [Key]
        /// <summary>
        /// 主键ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 入库单号
        /// </summary>
        public string Asn_No { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public string Customer_No { get; set; }

        /// <summary>
        /// 总单号
        /// </summary>
        public string Bill_No_All { get; set; }

        /// <summary>
        /// 分单号
        /// </summary>
        public string Bill_No_Split { get; set; }

        /// <summary>
        /// 成交方式
        /// </summary>
        public string Fob_Type { get; set; }

        /// <summary>
        /// 件数
        /// </summary>
        public string Dozen { get; set; }

        /// <summary>
        /// 包装种类
        /// </summary>
        public string Package_Type { get; set; }

        /// <summary>
        /// 币制
        /// </summary>
        public string Curr { get; set; }

    }
}

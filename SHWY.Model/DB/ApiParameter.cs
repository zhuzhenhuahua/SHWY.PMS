using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Model.DB
{
    [Serializable]
    [Table("ApiParameter")]
   public class ApiParameter
    {
        /// <summary>
        /// 主键ID，自增长
        /// </summary>
        [Key]
        public int ParaID { get; set; }
        /// <summary>
        /// 所属链接ID
        /// </summary>
        public int ApiUrlID { get; set; }
        /// <summary>
        /// 参数中文名
        /// </summary>
        public string CName { get; set; }
        /// <summary>
        /// 参数英文名
        /// </summary>
        public string EName { get; set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        public int DataType { get; set; }
        /// <summary>
        /// 输入参数或输出参数(1:输入参数2：输出参数)
        /// </summary>
        public int InOROutPut { get; set; }
        /// <summary>
        /// 是否允许为空（true:可为空，false:不可为空）
        /// </summary>
        public bool IsNull { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}

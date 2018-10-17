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
    [Table("ApiUrl")]
    public class ApiUrl
    {
        /// <summary>
        /// 自增长主键
        /// </summary>
        [Key]
        public int UrlID { get; set; }
        /// <summary>
        /// 所属产品ID
        /// </summary>
        public string ProdID { get; set; }
        /// <summary>
        /// API名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public int ParentID { get; set; }
        /// <summary>
        /// 请求地址链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// method：请求的类型；1:GET 或 2:POST
        /// </summary>
        public int method { get; set; }
        /// <summary>
        /// 排序ID
        /// </summary>
        public int SortID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        [NotMapped]
        public List<ApiUrl> children { get; set; }
        [NotMapped]
        public List<ApiParameter> apiParas { get; set; }

        [NotMapped]
        public string methodName { get; set; }
    }
}

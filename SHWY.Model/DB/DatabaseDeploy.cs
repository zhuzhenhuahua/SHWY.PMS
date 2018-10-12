using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Model.DB
{
    /// <summary>
    /// 数据库部署管理类
    /// </summary>
    [Serializable]
    [Table("databaseDeploy")]
   public class DatabaseDeploy
    {
        /// <summary>
        /// 自增长主键
        /// </summary>
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        /// <summary>
        /// 甲方公司ID
        /// </summary>
        public string partyID { get; set; }
        public int serverid { get; set; }
        /// <summary>
        /// 数据库架构ID，对应ECodesTypeId.databaseSchema
        /// </summary>
        public int schemaid { get; set; }
        /// <summary>
        /// 数据库类型，对应ECodesTypeId.databaseType
        /// </summary>
        public int type { get; set; }
        public string remark { get; set; }
        /// <summary>
        /// sql数据库名
        /// </summary>
        public string sqlServerCatlog { get; set; }
        public string mongoAdminDBName { get; set; }
        public string mongoDBName { get; set; }
        public string orclServiceName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}

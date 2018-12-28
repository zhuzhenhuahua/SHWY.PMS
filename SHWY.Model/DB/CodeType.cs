using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Model.DB
{
    [Serializable]
    [Table("CodeType")]
   public class CodeType
    {
        /// <summary>
        /// 自增长主键
        /// </summary>
        [Key]
        public int TypeId { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 类型描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否删除0未1已删除
        /// </summary>
        public int IsDelete { get; set; }
    }
    public enum ECodesTypeId
    {
        /// <summary>
        /// 服务态度
        /// </summary>
        [Description("服务态度")]
        serviceAttri = 1,
        /// <summary>
        /// 完成速度
        /// </summary>
        complSpeed = 2,
        /// <summary>
        /// 任务困难程度
        /// </summary>
        taskDiffLevel = 3,
        /// <summary>
        /// 完成程度
        /// </summary>
        taskComplDegree = 4,
        /// <summary>
        /// IP地址内外网标识
        /// </summary>
        IpAddressBeLong = 5,
        /// <summary>
        /// 通讯协议类型(TCP/UDP)
        /// </summary>
        ProtType = 6,
        /// <summary>
        /// 数据库架构
        /// </summary>
        databaseSchema = 7,
        /// <summary>
        /// 数据库类型（sql/oracle）
        /// </summary>
        databaseType = 8,
        /// <summary>
        /// 接口请求类型(post、get)
        /// </summary>
        MethodType = 9,
        /// <summary>
        /// API参数的数据类型
        /// </summary>
        DataTypeByApiPara = 10,
        /// <summary>
        /// 主题
        /// </summary>
        EasyuiThemes = 11
    }
}

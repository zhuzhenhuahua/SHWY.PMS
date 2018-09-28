using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Model.DB
{
    /// <summary>
    /// 统一字典表
    /// </summary>
    [Serializable]
    [Table("Codes")]
    public class Codes
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Text { get; set; }
        public int Status { get; set; }
        public int TypeId { get; set; }

    }
    public enum ECodesTypeId
    {
        /// <summary>
        /// 服务态度
        /// </summary>
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
        IpAddressBeLong = 5
    }
}

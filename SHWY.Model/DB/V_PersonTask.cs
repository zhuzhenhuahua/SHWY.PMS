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
    /// PersonTask的视图
    /// </summary>
    [Serializable]
    [Table("V_PersonTask")]
    public class V_PersonTask
    {
        [Key]
        public string ID { get; set; }
        /// <summary>
        /// JIRID
        /// </summary>
        public string JIRID { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public string itemID { get; set; }
        public string ItemName { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public string prodId { get; set; }
        public string ProdName { get; set; }
        /// <summary>
        /// 任务类型，对应TaskType表(需求/设计/开发/测试/部署/bug/维护)
        /// </summary>
        public int TaskType { get; set; }
        public string taskTypeName { get; set; }
        public string moduleName { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string detail { get; set; }
        /// <summary>
        /// 发布时间(视图转好的格式，直接显示)
        /// </summary>
        public string publishTime { get; set; }
        /// <summary>
        /// 原字段类型（datetime）
        /// </summary>
        public DateTime publishTimeDate { get; set; }
        /// <summary>
        /// 预期完成时间
        /// </summary>
        public string predDeadTime { get; set; }
        /// <summary>
        /// 当前任务状态，对应TaskStatus（未开始、解决中、已完成、完成但是存在问题、放弃）
        /// </summary>
        public int taskStatus { get; set; }
        public string taskStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 发布人
        /// </summary>
        public int publisherID { get; set; }
        public string publisherName { get; set; }
        /// <summary>
        /// 处理人
        /// </summary>
        public int handlerID { get; set; }
        public string handlerName { get; set; }
        /// <summary>
        /// 跟踪人
        /// </summary>
        public int followerID { get; set; }
        public string followerName { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public string complTime { get; set; }
        /// <summary>
        /// 任务完成度
        /// </summary>
        public string taskComplDegree { get; set; }
        public string taskComplDegreeName { get; set; }
        /// <summary>
        /// 服务态度
        /// </summary>
        public string serviceAttri { get; set; }
        public string serviceAttriName { get; set; }
        /// <summary>
        /// 完成速度
        /// </summary>
        public string complSpeed { get; set; }
        public string complSpeedName { get; set; }
        /// <summary>
        /// 任务困难程度
        /// </summary>
        public string taskDiffLevel { get; set; }
        public string taskDiffLevelName { get; set; }
        /// <summary>
        /// 评价描述
        /// </summary>
        public string evaDesc { get; set; }
    }
}

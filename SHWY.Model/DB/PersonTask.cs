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
    [Table("PersonTask")]
    public class PersonTask
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DisplayName("主键")]
        [Key]
        public string ID { get; set; }
        /// <summary>
        /// JIRID
        /// </summary>
        [DisplayName("JIRID")]
        public string JIRID { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        [DisplayName("项目ID")]
        public string itemID { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        [DisplayName("产品ID")]
        public string prodId { get; set; }
        /// <summary>
        /// 任务类型，对应TaskType表(需求/设计/开发/测试/部署/bug/维护)
        /// </summary>
        [DisplayName("任务类型")]
        public int TaskType { get; set; }
        /// <summary>
        /// 任务所属模块ID
        /// </summary>
        [DisplayName("所属模块")]
        public int ModuleID { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        [DisplayName("任务描述")]
        public string detail { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        [DisplayName("发布时间")]
        public DateTime publishTime { get; set; }
        /// <summary>
        /// 预计开始时间
        /// </summary>
        [DisplayName("预计开始时间")]
        public DateTime perdStartTime { get; set; }
        /// <summary>
        /// 预期完成时间
        /// </summary>
        [DisplayName("预期完成时间")]
        public DateTime predDeadTime { get; set; }
        /// <summary>
        /// 当前任务状态，对应TaskStatus（1未开始、2解决中、3已完成、4完成但是存在问题、5放弃）
        /// </summary>
        [DisplayName("当前任务状态")]
        public int taskStatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName("备注")]
        public string remark { get; set; }
        /// <summary>
        /// 发布人
        /// </summary>
        [DisplayName("发布人")]
        public int publisherID { get; set; }
        /// <summary>
        /// 处理人
        /// </summary>
        [DisplayName("处理人")]
        public int handlerID { get; set; }
        /// <summary>
        /// 跟踪人
        /// </summary>
        [DisplayName("跟踪人")]
        public int followerID { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        [DisplayName("完成时间")]
        public DateTime? complTime { get; set; }
        /// <summary>
        /// 任务完成度
        /// </summary>
        [DisplayName("任务完成度")]
        public string taskComplDegree { get; set; }
        /// <summary>
        /// 服务态度
        /// </summary>
        [DisplayName("服务态度")]
        public string serviceAttri { get; set; }
        /// <summary>
        /// 完成速度
        /// </summary>
        [DisplayName("完成速度")]
        public string complSpeed { get; set; }
        /// <summary>
        /// 任务困难程度
        /// </summary>
        [DisplayName("任务困难程度")]
        public string taskDiffLevel { get; set; }
        /// <summary>
        /// 评价描述
        /// </summary>
        [DisplayName("评价描述")]
        public string evaDesc { get; set; }
    }

    public enum ETaskStatus
    {
        未开始 = 1,
        解决中 = 2,
        已完成 = 3,
        完成但是存在问题 = 4,
        放弃 = 5
    }
}

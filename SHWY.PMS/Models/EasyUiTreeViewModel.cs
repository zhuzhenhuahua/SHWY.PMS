using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SHWY.PMS.Models
{
    public class EasyUiTreeViewModel
    {
        /// <summary>
        /// 节点的 id，它对于加载远程数据很重要。
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 要显示的节点文本。
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 指示节点是否被选中
        /// </summary>
        public bool @checked{get;set;}
        /// <summary>
        /// 定义了一些子节点的节点数组。
        /// </summary>
        public List<EasyUiTreeViewModel> children { get; set; }
    }
}
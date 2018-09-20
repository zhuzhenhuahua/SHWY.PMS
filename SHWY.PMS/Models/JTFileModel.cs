using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SHWY.PMS.Models
{
    public class JTFileModel
    {
        /// <summary>
        /// 文件的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extensioin { get; set; }
        /// <summary>
        /// 获取文件的全限定名称（完整路径）
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 获取文件的大小（字节数）
        /// </summary>
        public long Length { get; set; }
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

    }
}
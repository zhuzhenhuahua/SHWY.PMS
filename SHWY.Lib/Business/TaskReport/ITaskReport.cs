using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Lib.Business.TaskReport
{
    /// <summary>
    /// 生成日报/周报/月报的接口
    /// </summary>
    public interface ITaskReport
    {
        /// <summary>
        /// 创建报表
        /// </summary>
      Task<string> CreateReport(List<int> userIDs,DateTime reportDate);
    }
}

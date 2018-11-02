using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Utility
{
    public static  class AsyncHelper
    {
        /// <summary>
        /// 将一个方法function异步运行，在执行完毕时执行回调callback.
        /// </summary>
        /// <param name="funcToRun">不带参数的Action</param>
        /// <param name="callBack">不带参数的callBack</param>
        public static async void RunAsync(Action funcToRun, Action callBack)
        {
            Func<Task> taskFunc = () =>
            {
                return Task.Run(() =>
                {
                    funcToRun();
                });
            };
            await taskFunc();
            if (callBack != null)
                callBack();
        }
    }
}

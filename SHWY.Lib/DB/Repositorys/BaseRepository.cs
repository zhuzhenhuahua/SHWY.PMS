using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Lib.DB.Context;

namespace SHWY.Lib.DB.Repositorys
{
    public class BaseRepository : IDisposable
    {
        public readonly ProContext context;

        //public ProContext Context => context;
        static object _syncObject = new object();

        public BaseRepository()
        {
            context = new ProContext();
        }
        public BaseRepository(string conn)
        {
            context = new ProContext(conn);
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }
                disposedValue = true;
            }
        }

        // ~ProRepository() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        void IDisposable.Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region 公共虚方法

        #endregion
    }
}

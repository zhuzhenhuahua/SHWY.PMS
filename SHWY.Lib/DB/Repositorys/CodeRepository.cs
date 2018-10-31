using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB;
using System.Collections.Concurrent;
using System.Data.Entity;

namespace SHWY.Lib.DB.Repositorys
{
    /// <summary>
    /// 统一字典表的封装类
    /// </summary>
    public class CodeRepository : BaseRepository
    {
        private static CodeRepository codeRepository;
        private static readonly object locker = new object();
        /// <summary>
        /// 此处private不允许new对象，防止使用using释放资源
        /// </summary>
        private CodeRepository()
        {

        }
        public static CodeRepository CreateInstance()
        {
            if (codeRepository == null)
            {
                lock (locker)
                {
                    if (codeRepository == null)
                        codeRepository = new CodeRepository();
                }
            }
            return codeRepository;
        }

        #region TaskType
        public async Task<List<CodeTaskType>> GetTaskTypeListAsync()
        {
            var list = await (from j in context.TaskTypes
                              select j).ToListAsync();
            return list;
        }
        static ConcurrentDictionary<int, string> _dicTaskType = new ConcurrentDictionary<int, string>();
        public async Task<string> GetTaskTypeNameAsync(int id)
        {
            if (!_dicTaskType.ContainsKey(id))
            {
                var taskType = await (from j in context.TaskTypes.Where(p => p.ID == id)
                                      select j).FirstOrDefaultAsync();
                if (taskType != null)
                    _dicTaskType[id] = taskType.NAME;
                else
                    return string.Empty;
            }
            return _dicTaskType[id];
        }
        #endregion
        #region TaskStatus
        public async Task<List<CodeTaskStatus>> GetTaskStatusListAsync()
        {
            var list = await (from j in context.TaskStatus
                              select j).ToListAsync();
            return list;
        }
        static ConcurrentDictionary<int, string> _dicTaskStatus = new ConcurrentDictionary<int, string>();
        public async Task<string> GetTaskStatusNameAsync(int id)
        {
            if (!_dicTaskStatus.ContainsKey(id))
            {
                var taskStatus = await (from j in context.TaskStatus.Where(p => p.id == id)
                                        select j).FirstOrDefaultAsync();
                if (taskStatus != null)
                    _dicTaskStatus[id] = taskStatus.name;
                else
                    return string.Empty;
            }
            return _dicTaskStatus[id];
        }
        #endregion
        #region Codes
        static ConcurrentDictionary<int, List<Codes>> _dicCodes = new ConcurrentDictionary<int, List<Codes>>();
        public async Task<List<Codes>> GetCodesListAsync(ECodesTypeId typeId)
        {
            int typeid = Convert.ToInt32(typeId);
            if (!_dicCodes.ContainsKey(typeid))
            {
                var list = await (from j in context.Codes
                                  where j.TypeId == typeid && j.Status == 1
                                  select j).ToListAsync();
                _dicCodes[typeid] = list;
                return list;
            }
            return _dicCodes[typeid];

        }
        #endregion
    }
}

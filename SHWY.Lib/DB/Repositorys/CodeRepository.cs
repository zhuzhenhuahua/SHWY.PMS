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
            if (_dicCodes.ContainsKey(typeid))
            {
                return _dicCodes[typeid];
            }
            var list = await (from j in context.Codes
                              where j.TypeId == typeid && j.Status == 1
                              select j).ToListAsync();
            if (list.Count > 0)
            {
                _dicCodes[typeid] = list;
            }
            return list;
        }
        public async Task<List<Codes>> GetCodesListAsync(int typeId)
        {
            var list = await (from j in context.Codes
                              where j.TypeId == typeId
                              select j).ToListAsync();
            return list;
        }
        public async Task<string> GetCodesTextAsync(ECodesTypeId typeid, string code)
        {
            var list = await GetCodesListAsync(typeid);
            if (list == null || list.Count == 0)
                return string.Empty;
            var codeModel = list.Where(p => p.Code == code).FirstOrDefault();
            if (codeModel != null)
            {
                return codeModel.Text;
            }
            int typID = Convert.ToInt32(typeid);
            var codeList = await (from j in context.Codes
                                  where j.TypeId == typID
                                  select j).ToListAsync();
            if (codeList.Count>0)
            {
                _dicCodes[typID] = codeList;
                return codeList.Where(p => p.Code == code).FirstOrDefault()?.Text;
            }
            return string.Empty;
        }
        public async Task<Codes> GetCodeAsync(int codeId)
        {
            var model = await context.Codes.Where(p => p.Id == codeId).FirstOrDefaultAsync();
            return model;
        }
        public async Task<bool> AddOrUpdateCode(Codes codePara)
        {
            var isAdd = false;
            var model = await context.Codes.Where(p => p.Id == codePara.Id).FirstOrDefaultAsync();
            if (model == null)
            {
                isAdd = true;
                model = new Codes() { TypeId = codePara.TypeId };
            }
            model.Code = codePara.Code;
            model.Text = codePara.Text;
            model.Status = codePara.Status;
            if (isAdd)
                context.Codes.Add(model);
            int res = await context.SaveChangesAsync();
            if (res > 0)
            {
                var listCode = new List<Codes>();
                _dicCodes.TryRemove(model.TypeId, out listCode);
            }
            return res>0;
        }
        #endregion
    }
}

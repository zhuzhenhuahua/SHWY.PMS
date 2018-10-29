using SHWY.Model.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Lib.DB.Repositorys
{
    public class ApiManaRepository : BaseRepository
    {
        #region APIBASEURL
        public async Task<Tuple<int, List<ApiBaseUrl>>> GetApiBaseUrlListAsync(int page, int rows)
        {
            int from = (page - 1) * rows;
            var total = await (from j in context.ApiBaseUrls
                               select j).CountAsync();
            var list = await (from j in context.ApiBaseUrls
                              orderby j.Id descending
                              select j).Skip(from).Take(rows).ToListAsync();
            return Tuple.Create(total, list);
        }
        public async Task<List<ApiBaseUrl>> GetApiBaseUrlListAsync()
        {
            var list = await context.ApiBaseUrls.ToListAsync();
            return list;
        }
        public async Task<ApiBaseUrl> GetApiBaseUrlAsync(int Id)
        {
            var model = await context.ApiBaseUrls.Where(p => p.Id == Id).FirstOrDefaultAsync();
            return model;
        }
        public async Task<bool> AddOrUpdateApiBaseUrlAsync(ApiBaseUrl para)
        {
            var isAdd = false;
            var model = await context.ApiBaseUrls.Where(p => p.Id == para.Id).FirstOrDefaultAsync();
            if (model == null)
            {
                isAdd = true;
                model = new ApiBaseUrl();
            }
            model.BaseName = para.BaseName;
            model.BaseUrl = para.BaseUrl;
            model.Remark = para.Remark;
            if (isAdd)
                context.ApiBaseUrls.Add(model);
            return await context.SaveChangesAsync() == 1;
        }
        public async Task<bool> DelApiBaseUrlAsync(int Id)
        {
            var model = await context.ApiBaseUrls.Where(p => p.Id == Id).FirstOrDefaultAsync();
            if (model != null)
            {
                context.ApiBaseUrls.Remove(model);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
        #endregion
        #region APIURL
        public async Task<Tuple<int, List<ApiUrl>>> GetApiUrlListAsync(int page, int rows, string name, int parentId)
        {
            int from = (page - 1) * rows;
            var total = await (from j in context.ApiUrls
                               where (j.Name.Contains(name)) && (parentId > 0 ? j.ParentID == parentId : 1 == 1)
                               select j).CountAsync();
            var list = await (from j in context.ApiUrls
                              where (j.Name.Contains(name)) && (parentId > 0 ? j.ParentID == parentId : 1 == 1)
                              orderby j.SortID
                              select j).Skip(from).Take(rows).ToListAsync();
            return Tuple.Create(total, list);
        }
        public async Task<List<ApiUrl>> GetApiUrlTreeList(int parentId)
        {
            var list = await context.ApiUrls.OrderBy(p => p.SortID).ToListAsync();
            List<ApiUrl> treeList = new List<ApiUrl>();
            treeList = list.Where(p => p.ParentID == parentId).ToList();
            var methodList = await CodeRepository.CreateInstance().GetCodesListAsync(ECodesTypeId.MethodType);
            foreach (var item in treeList)
            {
                item.methodName = string.Empty;
                item.children = list.Where(p => p.ParentID == item.UrlID).ToList();
                foreach (var child in item.children)
                {
                    child.methodName = methodList.Where(p => p.Code == child.method.ToString()).FirstOrDefault()?.Text;
                }
            }
            return treeList;
        }
        public async Task<ApiUrl> GetApiUrlAsync(int urlID)
        {
            var model = await context.ApiUrls.Where(p => p.UrlID == urlID).FirstOrDefaultAsync();
            return model;
        }
        public async Task<List<ApiUrl>> GetApiUrlListByParentID(int parentID)
        {
            var list = await context.ApiUrls.Where(p => p.ParentID == parentID).ToListAsync();
            return list;
        }
        public async Task<bool> AddOrUpdateApiUrlAsync(ApiUrl para)
        {
            var isAdd = false;
            var model = await context.ApiUrls.Where(p => p.UrlID == para.UrlID).FirstOrDefaultAsync();
            if (model == null)
            {
                isAdd = true;
                model = new ApiUrl();
            }
            model.ProdID = para.ProdID;
            model.Name = para.Name;
            model.ParentID = para.ParentID;
            model.Url = para.Url;
            model.method = para.method;
            model.SortID = para.SortID;
            model.Remark = para.Remark;
            if (isAdd)
                context.ApiUrls.Add(model);
            return await context.SaveChangesAsync() == 1;
        }
        public async Task<bool> DelApiUrlAsync(int urlID)
        {
            var model = await context.ApiUrls.Where(p => p.UrlID == urlID).FirstOrDefaultAsync();
            if (model != null)
            {
                context.ApiUrls.Remove(model);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
        #endregion

        #region APIPARA
        public async Task<Tuple<int, object>> GetApiParaListAsync(int pageIndex, int pageSize, int urlID, int inputOrOupPut)
        {
            int form = (pageIndex - 1) * pageSize;
            int total = await (from j in context.ApiParameters
                               where j.ApiUrlID == urlID
                               && (inputOrOupPut == 0 ? 1 == 1 : j.InOROutPut == inputOrOupPut)
                               select j).CountAsync();
            var obj = await (from j in context.ApiParameters
                             join dType in context.Codes.Where(p => p.TypeId == (int)ECodesTypeId.DataTypeByApiPara) on j.DataType.ToString() equals dType.Code into tempDataType
                             from dataType in tempDataType.DefaultIfEmpty()
                             where j.ApiUrlID == urlID
                              && (inputOrOupPut == 0 ? 1 == 1 : j.InOROutPut == inputOrOupPut)
                             orderby j.ParaID descending
                             select new
                             {
                                 j.ParaID,
                                 j.ApiUrlID,
                                 j.CName,
                                 j.EName,
                                 DataType = dataType.Text,
                                 j.InOROutPut,
                                 j.IsNull,
                                 j.Remark
                             }).Skip(form).Take(pageSize).ToListAsync();
            return Tuple.Create<int, object>(total, obj);
        }
        public async Task<List<ApiParameter>> GetApiParaListAsync(int urlID)
        {
            var list = await context.ApiParameters.Where(p => p.ApiUrlID == urlID).ToListAsync();
            return list;
        }
        public async Task<List<ApiParameter>> GetApiParaListAsync(int urlID, int inOutPut)
        {
            var list = await context.ApiParameters.Where(p => p.ApiUrlID == urlID && p.InOROutPut == inOutPut).ToListAsync();
            return list;
        }
        public async Task<ApiParameter> GetApiParaByParaIDAsync(int urlID, int paraID)
        {
            var model = await context.ApiParameters.Where(p => p.ApiUrlID == urlID && p.ParaID == paraID).FirstOrDefaultAsync();
            return model;
        }
        public async Task<bool> AddOrUpdateApiParaAsync(ApiParameter para)
        {
            var isAdd = false;
            var model = await context.ApiParameters.Where(p => p.ApiUrlID == para.ApiUrlID && p.ParaID == para.ParaID).FirstOrDefaultAsync();
            if (model == null)
            {
                isAdd = true;
                model = new ApiParameter() { ApiUrlID = para.ApiUrlID };
            }
            model.CName = para.CName;
            model.EName = para.EName;
            model.DataType = para.DataType;
            model.InOROutPut = para.InOROutPut;
            model.IsNull = para.IsNull;
            model.Remark = para.Remark;
            if (isAdd)
                context.ApiParameters.Add(model);
            return await context.SaveChangesAsync() == 1;
        }
        public async Task<bool> DelApiParaAsync(int urlID, int paraID)
        {
            var model = await context.ApiParameters.Where(p => p.ApiUrlID == urlID && p.ParaID == paraID).FirstOrDefaultAsync();
            if (model != null)
            {
                context.ApiParameters.Remove(model);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
        #endregion
    }
}

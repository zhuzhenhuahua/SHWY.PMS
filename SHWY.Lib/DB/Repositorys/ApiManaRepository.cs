using SHWY.Model.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Lib.DB.Repositorys
{
   public class ApiManaRepository:BaseRepository
    {
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
                    child.methodName = methodList.Where(p => p.Code ==child.method.ToString()).FirstOrDefault()?.Text;
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
            if (model!=null)
            {
                context.ApiUrls.Remove(model);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
        #endregion

        #region APIPARA
        public async Task<List<ApiParameter>> GetApiParaListAsync(int urlID)
        {
            var list = await context.ApiParameters.Where(p => p.ApiUrlID == urlID).ToListAsync();
            return list;
        }
        #endregion
    }
}

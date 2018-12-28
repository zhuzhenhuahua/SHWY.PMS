using SHWY.Model.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Lib.DB.Repositorys
{
    public class CodeTypeRepository : BaseRepository
    {
        #region CodeType
        public async Task<Tuple<int, List<CodeType>>> GetCodeTypeListAsync(int page, int rows, string typeName)
        {
            int pageIndex = (page - 1) * rows;
            var total = await (from j in context.CodeTypes
                               where j.IsDelete == 0
                               && (string.IsNullOrEmpty(typeName) ? 1 == 1 : j.TypeName.Contains(typeName))
                               select j).CountAsync();
            var list = await (from j in context.CodeTypes
                              where j.IsDelete == 0
                              && (string.IsNullOrEmpty(typeName) ? 1 == 1 : j.TypeName.Contains(typeName))
                              orderby j.TypeId descending
                              select j).Skip(pageIndex).Take(rows).ToListAsync();
            return Tuple.Create(total, list);
        }
        public async Task<CodeType> GetCodeTypeAsync(int typeId)
        {
            var model = await context.CodeTypes.Where(p => p.TypeId == typeId).FirstOrDefaultAsync();
            return model;
        }
        public async Task<bool> AddOrUpdateAsync(CodeType modelPara)
        {
            var isAdd = false;
            var model = await context.CodeTypes.Where(p => p.TypeId == modelPara.TypeId).FirstOrDefaultAsync();
            if (model == null)
            {
                isAdd = true;
                model = new CodeType() { IsDelete = 0 };
            }
            model.TypeName = modelPara.TypeName;
            model.Remark = modelPara.Remark;
            if (isAdd)
                context.CodeTypes.Add(model);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DelCodeTypeAsync(int typeID)
        {
            var model = await context.CodeTypes.Where(p => p.TypeId == typeID).FirstOrDefaultAsync();
            if (model != null)
            {
                model.IsDelete = 1;
                return await context.SaveChangesAsync() > 0;
            }
            return false;
        }
        #endregion
    }
}

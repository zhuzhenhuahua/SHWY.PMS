using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB;

namespace SHWY.Lib.DB.Repositorys
{
   public class ItemsRepository:BaseRepository
    {
        public async Task<Tuple<int, List<Items>>> GetListAsync(int pageIndex, int pageSize, string itemName)
        {
            int from = (pageIndex - 1) * pageSize;
            int total = await (from j in context.Items
                               where itemName == "" ? 1 == 1 : j.NAME.Contains(itemName)
                               select j).CountAsync();
            var list = await (from j in context.Items
                              where itemName == "" ? 1 == 1 : j.NAME.Contains(itemName)
                              orderby j.ItemID descending
                              select j).Skip(from).Take(pageSize).ToListAsync();
            return Tuple.Create(total, list);
        }
        public async Task<Items> GetItemAsync(int itemId)
        {
            try
            {
                var item = await (from j in context.Items
                                  where j.ItemID == itemId
                                  select j).FirstOrDefaultAsync();
                if (item == null)
                    return new Items();
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region 增删改
        public async Task<bool> AddOrUpdateAsync(Items item)
        {
            try
            {
                var itemNew = await GetItemAsync(item.ItemID);
                bool isNew = false;

                if (itemNew==null|| itemNew.ItemID.Equals(0))
                {
                    isNew = true;
                }
                foreach (var p in itemNew.GetType().GetProperties())
                {
                    //更新属性
                    var v = item.GetType().GetProperty(p.Name).GetValue(item);
                    if (v != null)
                    {
                        //其他字段更新
                        p.SetValue(itemNew, v);
                    }
                }
                if (isNew)
                    context.Items.Add(item);
                return await context.SaveChangesAsync() == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteItem(int itemId)
        {
            var item = await context.Items.Where(p => p.ItemID == itemId).FirstOrDefaultAsync();
            if (item != null)
            {
                context.Items.Remove(item);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
        #endregion
    }
}

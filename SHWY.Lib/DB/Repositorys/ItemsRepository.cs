using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB;
using System.Collections.Concurrent;

namespace SHWY.Lib.DB.Repositorys
{
    public class ItemsRepository : BaseRepository
    {
        private static ItemsRepository _itemsRepository;
        private static readonly object locker = new object();
        private ItemsRepository()
        {

        }
        public static ItemsRepository CreateInstance()
        {
            if (_itemsRepository == null)
            {
                lock (locker)
                {
                    if (_itemsRepository == null)
                        _itemsRepository = new ItemsRepository();
                }
            }
            return _itemsRepository;
        }
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
        public async Task<List<Items>> GetListItemsAsync()
        {
            var list = await (from j in context.Items
                              select j).ToListAsync();
            return list;
        }
        public static ConcurrentDictionary<int, Items> _dicItem = new ConcurrentDictionary<int, Items>();
        public async Task<Items> GetItemAsync(int itemId)
        {
            var item = await (from j in context.Items
                              where j.ItemID == itemId
                              select j).FirstOrDefaultAsync();
            return item;

        }
        public async Task<Items> GetItemDicAsync(int itemId)
        {
            try
            {
                if (!_dicItem.ContainsKey(itemId))
                {
                    var item = await (from j in context.Items
                                      where j.ItemID == itemId
                                      select j).FirstOrDefaultAsync();
                    if (item != null)
                        _dicItem[itemId] = item;
                    else
                        return null;
                }
                return _dicItem[itemId];
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

                if (itemNew == null || itemNew.ItemID.Equals(0))
                {
                    isNew = true;
                    itemNew = new Items();
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
            catch (Exception ex)
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

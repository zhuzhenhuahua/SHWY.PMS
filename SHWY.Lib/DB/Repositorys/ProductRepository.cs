using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB;

namespace SHWY.Lib.DB.Repositorys
{
    public class ProductRepository : BaseRepository
    {
        public async Task<Tuple<int, List<Product>>> GetListAsync(int rows, int page, string prodName)
        {
            int form = (rows - 1) * page;
            var total = await (from j in context.Products
                               where prodName == "" ? 1 == 1 : j.NAME.Contains(prodName)
                               select j).CountAsync();
            var list = await (from j in context.Products
                              where prodName == "" ? 1 == 1 : j.NAME.Contains(prodName)
                              orderby j.ProID
                              select j).Skip(form).Take(page).ToListAsync();
            return Tuple.Create(total, list);
        }
        public async Task<Product> GetProductAsync(string prodId)
        {
            try
            {
                var prod = await (from j in context.Products
                                  where j.ProID == prodId
                                  select j).FirstOrDefaultAsync();
                if (prod == null)
                    return new Product();
                return prod;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region 增删改
        public async Task<bool> AddOrUpdateAsync(Product prod)
        {
            try
            {
                var prodNew = await GetProductAsync(prod.ProID);
                bool isNew = false;

                if (prodNew == null ||string.IsNullOrEmpty(prodNew.ProID))
                {
                    isNew = true;
                }
                foreach (var p in prodNew.GetType().GetProperties())
                {
                    //更新属性
                    var v = prod.GetType().GetProperty(p.Name).GetValue(prod);
                    if (v != null)
                    {
                        //其他字段更新
                        p.SetValue(prodNew, v);
                    }
                }
                if (isNew)
                    context.Products.Add(prod);
                return await context.SaveChangesAsync() == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteProd(string proId)
        {
            var prod = await context.Products.Where(p => p.ProID == proId).FirstOrDefaultAsync();
            if (prod != null)
            {
                context.Products.Remove(prod);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
        #endregion
    }
}

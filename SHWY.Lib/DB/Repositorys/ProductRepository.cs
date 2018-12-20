using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB;
using SHWY.Model.Join;
using System.Collections.Concurrent;

namespace SHWY.Lib.DB.Repositorys
{
    public class ProductRepository : BaseRepository
    {
        private static ProductRepository _productRepository;
        private static readonly object locker = new object();
        private ProductRepository()
        {

        }
        public static ProductRepository CreateInstance()
        {
            if (_productRepository == null)
            {
                lock (locker)
                {
                    if (_productRepository == null)
                        _productRepository = new ProductRepository();
                }
            }
            return _productRepository;
        }

        #region Prod查询
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
        public async Task<List<Product>> GetListAsync()
        {
            var list = await (from j in context.Products
                              select j).ToListAsync();
            return list;
        }
        public static ConcurrentDictionary<string, Product> _dicProd = new ConcurrentDictionary<string, Product>();
        public async Task<Product> GetProductDicAsync(string prodId)
        {
            try
            {
                if (!_dicProd.ContainsKey(prodId))
                {
                    var prod = await (from j in context.Products
                                      where j.ProID == prodId
                                      select j).FirstOrDefaultAsync();
                    if (prod != null)
                        _dicProd[prodId] = prod;
                    else
                        return null;
                }
                return _dicProd[prodId];
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        #endregion

        #region ProdDBDeploy 查询
        public async Task<Tuple<int, object>> GetProdDBListAsync(int pageIndex, int pageSize, string itemid, string prodid, int dbid)
        {
            int form = (pageIndex - 1) * pageSize;
            var total = await (from j in context.ProdDBDeploys
                               where (string.IsNullOrEmpty(itemid) ? 1 == 1 : j.itemId == itemid)
                               && (string.IsNullOrEmpty(prodid) ? 1 == 1 : j.prodId == prodid)
                               && (dbid == 0 ? 1 == 1 : j.dbId == dbid)
                               select j).CountAsync();
            var list = await (from j in context.ProdDBDeploys
                              join item in context.Items on j.itemId equals item.ItemID
                              join prod in context.Products on j.prodId equals prod.ProID
                              join db in context.DatabaseDeploys on j.dbId equals db.id
                              join codeType in context.Codes.Where(p => p.TypeId == (int)ECodesTypeId.databaseType) on db.type.ToString() equals codeType.Code
                              where (string.IsNullOrEmpty(itemid) ? 1 == 1 : j.itemId == itemid)
                                && (string.IsNullOrEmpty(prodid) ? 1 == 1 : j.prodId == prodid)
                                && (dbid == 0 ? 1 == 1 : j.dbId == dbid)
                              orderby j.id descending
                              select new
                              {
                                  j.id,
                                  j.itemId,
                                  itemName = item.NAME,
                                  j.prodId,
                                  prodName = prod.NAME,
                                  j.dbId,
                                  dbNameCn = db.name,
                                  dbType = codeType.Text,
                                  db.sqlServerCatlog,
                                  db.mongoAdminDBName,
                                  db.mongoDBName,
                                  db.orclServiceName,
                                  db.username,
                                  db.password
                              }).Skip(form).Take(pageSize).ToListAsync();
            return Tuple.Create<int, object>(total, list);
        }
        public async Task<List<ItemProdDB>> GetProdDBListAsync(string itemID)
        {
            var list = await (from j in context.ProdDBDeploys
                              join db in context.DatabaseDeploys on j.dbId equals db.id into tempDB
                              from dbs in tempDB.DefaultIfEmpty()
                              where j.itemId == itemID
                              select new ItemProdDB
                              {
                                  prodID = j.prodId,
                                  database = dbs
                              }).ToListAsync();
            return list;
        }
        public async Task<ProdDBDeploy> GetProdDBDeployAsync(int id)
        {
            var model = await context.ProdDBDeploys.Where(p => p.id == id).FirstOrDefaultAsync();
            return model;
        }
        public async Task<int> GetProdDBDeployCountByDBIDAsync(int dbid)
        {
            var total = await context.ProdDBDeploys.Where(p => p.dbId == dbid).CountAsync();
            return total;
        }
        #endregion

        #region ProdServer（ProdDeploy）查询
        public async Task<Tuple<int, object>> GetProdServerDeployListAsync(int pageIndex, int pageSize, string prodID, int serverID, string itemID)
        {
            int form = (pageIndex - 1) * pageSize;
            var total = await (from j in context.ProdServerDeploys
                               where (string.IsNullOrEmpty(prodID) ? 1 == 1 : j.prodid == prodID)
                               && (serverID == 0 ? 1 == 1 : j.serverid == serverID)
                               && (string.IsNullOrEmpty(itemID) ? 1 == 1 : j.itemid == itemID)
                               select j).CountAsync();
            var list = await (from j in context.ProdServerDeploys
                              join item in context.Items on j.itemid equals item.ItemID into tempItem
                              from items in tempItem.DefaultIfEmpty()
                              join prod in context.Products on j.prodid equals prod.ProID into tempProd
                              from prods in tempProd.DefaultIfEmpty()
                              join server in context.Servers on j.serverid equals server.sid into tempServer
                              from servers in tempServer.DefaultIfEmpty()
                              join codes in context.Codes.Where(p => p.TypeId == 6) on j.porttype.ToString() equals codes.Code
                              where (string.IsNullOrEmpty(prodID) ? 1 == 1 : j.prodid == prodID)
                                   && (serverID == 0 ? 1 == 1 : j.serverid == serverID)
                                   && (string.IsNullOrEmpty(itemID) ? 1 == 1 : j.itemid == itemID)
                              orderby j.id descending
                              select new
                              {
                                  j.id,
                                  j.itemid,
                                  itemName = items.NAME,
                                  j.prodid,
                                  prodName = prods.NAME,
                                  j.serverid,
                                  serverName = servers.name,
                                  j.port,
                                  portTypeName = codes.Text,
                                  j.remark
                              }).Skip(form).Take(pageSize).ToListAsync();
            return Tuple.Create<int, object>(total, list);
        }
        public async Task<List<ItemProdServer>> GetProdServerDeployListByItemIDAsync(string itemID)
        {
            var list = await (from j in context.ProdServerDeploys
                              join prod in context.Products on j.prodid equals prod.ProID into tempProd
                              from prods in tempProd.DefaultIfEmpty()
                              join server in context.Servers on j.serverid equals server.sid into tempServer
                              from servers in tempServer.DefaultIfEmpty()
                              join codes in context.Codes.Where(p => p.TypeId == (int)ECodesTypeId.ProtType) on j.porttype.ToString() equals codes.Code
                              where j.itemid == itemID
                              orderby j.prodid
                              select new ItemProdServer
                              {
                                  porttypeName = codes.Text,
                                  prod = prods,
                                  server = servers,
                              }).ToListAsync();
            return list;
        }
        public async Task<ProdServerDeploy> GetProdServerDeployAsync(int id)
        {
            var model = await context.ProdServerDeploys.Where(p => p.id == id).FirstOrDefaultAsync();
            return model;
        }
        public async Task<int> GetProdServerCountByServerIDAsync(int serverid)
        {
            var total = await context.ProdServerDeploys.Where(p => p.serverid == serverid).CountAsync();
            return total;
        }
        #endregion

        #region ProdModule查询
        public async Task<Tuple<int, object>> GetProdModuleList(int pageIndex, int pageSize, string prodid)
        {
            int form = (pageIndex - 1) * pageSize;
            var total = await (from j in context.ProdModules
                               where j.ProID.Contains(prodid)
                               select j).CountAsync();
            var list = await (from j in context.ProdModules
                              join prod in context.Products on j.ProID equals prod.ProID
                              where j.ProID.Contains(prodid)
                              orderby j.ProID, j.ModuleID descending
                              select new
                              {
                                  j.ModuleID,
                                  j.ProID,
                                  prodName = prod.NAME,
                                  j.NAME,
                                  j.Description
                              }).Skip(form).Take(pageSize).ToListAsync();
            return Tuple.Create<int, object>(total, list);
        }
        public async Task<ProdModule> GetProdModuleAsync(int moduledid, string prodid)
        {
            var model = await (from j in context.ProdModules
                               where j.ModuleID == moduledid && j.ProID == prodid
                               select j).FirstOrDefaultAsync();
            return model;
        }
        public async Task<int> GetProdModuleCountByProdIDAsync(string prodID)
        {
            var total = await context.ProdModules.Where(p => p.ProID == prodID).CountAsync();
            return total;
        }
        public async Task<List<ProdModule>> GetProdModuleListAsync(string prodID)
        {
            var list = await context.ProdModules.Where(p => p.ProID == prodID).ToListAsync();
            return list;
        }

        #endregion

        #region 增删改
        #region Prod增删改
        public async Task<bool> AddOrUpdateAsync(Product prod)
        {
            try
            {
                var prodNew = await GetProductAsync(prod.ProID);
                bool isNew = false;

                if (prodNew == null || string.IsNullOrEmpty(prodNew.ProID))
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

        #region ProdDBDeploy（ProdDeploy）增删改
        public async Task<bool> AddOrUpdateProdDBDeployAsync(ProdDBDeploy dbDeploy)
        {
            var isAdd = false;
            var model = await context.ProdDBDeploys.Where(p => p.id == dbDeploy.id).FirstOrDefaultAsync();
            if (model == null)
            {
                isAdd = true;
                model = new ProdDBDeploy();
            }
            model.partyID = dbDeploy.partyID;
            model.itemId = dbDeploy.itemId;
            model.prodId = dbDeploy.prodId;
            model.dbId = dbDeploy.dbId;
            if (isAdd)
                context.ProdDBDeploys.Add(model);
            return await context.SaveChangesAsync() == 1;
        }
        public async Task<bool> DelProdDBDeployAsync(int id)
        {
            var model = await context.ProdDBDeploys.Where(p => p.id == id).FirstOrDefaultAsync();
            if (model != null)
            {
                context.ProdDBDeploys.Remove(model);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
        #endregion

        #region ProdServer（ProdDeploy）增删改
        public async Task<bool> AddOrUpdateProdServerDeployAsync(ProdServerDeploy pDeployPara)
        {
            var isAdd = false;
            var model = await context.ProdServerDeploys.Where(p => p.id == pDeployPara.id).FirstOrDefaultAsync();
            if (model == null)
            {
                isAdd = true;
                model = new ProdServerDeploy();
            }
            model.partyID = pDeployPara.partyID;
            model.itemid = pDeployPara.itemid;
            model.prodid = pDeployPara.prodid;
            model.serverid = pDeployPara.serverid;
            model.port = pDeployPara.port;
            model.porttype = pDeployPara.porttype;
            model.remark = pDeployPara.remark;
            if (isAdd)
                context.ProdServerDeploys.Add(model);
            return await context.SaveChangesAsync() == 1;
        }
        public async Task<bool> DelProdServerDeployAsync(int id)
        {
            var model = await context.ProdServerDeploys.Where(p => p.id == id).FirstOrDefaultAsync();
            if (model != null)
            {
                context.ProdServerDeploys.Remove(model);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
        #endregion

        #region ProdModule增删改
        public async Task<bool> AddOrUpdateProdModuleAsync(ProdModule modulePara)
        {
            var isAdd = false;
            var model = await context.ProdModules.Where(p => p.ModuleID == modulePara.ModuleID && p.ProID == modulePara.ProID).FirstOrDefaultAsync();
            if (model == null)
            {
                isAdd = true;
                model = new ProdModule()
                {
                    ProID = modulePara.ProID
                };
            }
            model.NAME = modulePara.NAME;
            model.Description = modulePara.Description;
            if (isAdd)
                context.ProdModules.Add(model);
            return await context.SaveChangesAsync() == 1;
        }
        public async Task<bool> DelProdModuleAsync(int moduleId, string prodId)
        {
            var model = await context.ProdModules.Where(p => p.ModuleID == moduleId && p.ProID == prodId).FirstOrDefaultAsync();
            if (model != null)
            {
                context.ProdModules.Remove(model);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
        #endregion
        #endregion
    }
}

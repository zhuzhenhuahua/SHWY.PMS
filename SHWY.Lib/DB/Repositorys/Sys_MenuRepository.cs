﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB;

namespace SHWY.Lib.DB.Repositorys
{
    public class Sys_MenuRepository : BaseRepository
    {
        public async Task<List<Sys_Menu>> GetListAsync()
        {
            var list = await (from j in context.Sys_Menus
                              orderby j.MenuId, j.MenuSortID
                              select j).ToListAsync();
            return list;
        }
        public async Task<Tuple<int, List<Sys_Menu>>> GetListAsync(int page, int rows, string menuName, int parentId)
        {
            int from = (page - 1) * rows;
            var total = await (from j in context.Sys_Menus
                               where j.MenuName.Contains(menuName) && parentId > 0 ? j.ParentId == parentId : 1 == 1
                               select j).CountAsync();
            var list = await (from j in context.Sys_Menus
                              where j.MenuName.Contains(menuName) && parentId > 0 ? j.ParentId == parentId : 1 == 1
                              orderby j.MenuId, j.MenuSortID
                              select j).Skip(from).Take(rows).ToListAsync();
            return Tuple.Create(total, list);
        }
        public async Task<Sys_Menu> GetMenuAsync(int menuId)
        {
            return await context.Sys_Menus.Where(p => p.MenuId == menuId).FirstOrDefaultAsync();
        }
        public async Task<bool> AddOrUpdateAsync(Sys_Menu menu)
        {
            try
            {
                var sysMenu = await GetMenuAsync(menu.MenuId);
                bool isNew = false;

                if (sysMenu == null)
                {
                    isNew = true;
                    sysMenu = new Sys_Menu();
                }
                foreach (var p in sysMenu.GetType().GetProperties())
                {
                    //更新属性
                    var v = menu.GetType().GetProperty(p.Name).GetValue(menu);
                    if (v != null)
                    {
                        //其他字段更新
                        p.SetValue(sysMenu, v);
                    }
                }
                if (isNew)
                    context.Sys_Menus.Add(sysMenu);
                return await context.SaveChangesAsync() == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> DeleteMenuAsync(int menuId)
        {
            var menu = await context.Sys_Menus.Where(p => p.MenuId == menuId).FirstOrDefaultAsync();
            if (menu != null)
            {
                context.Sys_Menus.Remove(menu);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
        public async Task<List<Sys_Menu>> GetListByParentIdAsync(int parentId)
        {
            return await context.Sys_Menus.Where(p => p.ParentId == parentId).ToListAsync();
        }
        public async Task<List<Sys_Menu>> GetAllChildMeunList()
        {
            return await context.Sys_Menus.Where(p => p.ParentId > 0).ToListAsync();
        }
    }
}

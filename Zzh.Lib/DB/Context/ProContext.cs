using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zzh.Model.DB;

namespace Zzh.Lib.DB.Context
{
    public class ProContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        }
        public ProContext() : base("name=DefaultConnection")
        {
            Init();
        }
        public ProContext(string conn) : base(conn)
        {
            Init();
        }
        private void Init()
        {
            try
            {
                this.Configuration.LazyLoadingEnabled = true;
                this.Database.Initialize(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DbSet<EntAppAsnHead> EntAppAsnHeads { get; set; }
        public DbSet<EntApp_SJ_Head> EndAppSjHeads { get; set; }
        public DbSet<Sys_User> Sys_Users { get; set; }
        public DbSet<Sys_Role> Sys_Roles { get; set; }
        public DbSet<Sys_Menu> Sys_Menus { get; set; }
        public DbSet<Sys_RoleMenu> Sys_RoleMenus { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}

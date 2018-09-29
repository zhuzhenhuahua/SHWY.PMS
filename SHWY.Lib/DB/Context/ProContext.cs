using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHWY.Model.DB;
using SHWY.Model.DB.Configuration;

namespace SHWY.Lib.DB.Context
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
        public DbSet<Sys_Configuration> SysConfigurations { get; set; }
        public DbSet<Sys_User> Sys_Users { get; set; }
        public DbSet<Sys_Role> Sys_Roles { get; set; }
        public DbSet<Sys_Menu> Sys_Menus { get; set; }
        public DbSet<Sys_MenuOper> Sys_MenuOpers { get; set; }
        public DbSet<Sys_RoleMenu> Sys_RoleMenus { get; set; }
        public DbSet<Sys_RoleOper> Sys_RoleOpers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProdDeploy> ProdDeploys { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<PersonTask> PersonTasks { get; set; }
        public DbSet<V_PersonTask> VPersonTask { get; set; }
        public DbSet<CodeTaskStatus> TaskStatus { get; set; }
        public DbSet<CodeTaskType> TaskTypes { get; set; }
        public DbSet<Codes> Codes { get; set; }
        public DbSet<PersonTaskProcess> PersonTaskProcess { get; set; }
        public DbSet<Servers> Servers { get; set; }
        public DbSet<IpAddress> IpAddress { get; set; }
        public DbSet<ServerIp> ServerIps { get; set; }
    }
}

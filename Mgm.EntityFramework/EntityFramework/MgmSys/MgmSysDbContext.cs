using Abp.EntityFramework;
using Mgm.MgmSys.MgmUser;
using System.Data.Entity;
using Mgm.MgmSys.MgmUserGroup;
using Mgm.Test;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Mgm.EntityFramework.MgmSys
{
    public class MgmSysDbContext : AbpDbContext
    {
        public virtual IDbSet<MgmUsers> MgmUsers { get; set; }
        public virtual IDbSet<MgmUserGroup> MgmUserGroups { get; set; }
        public virtual IDbSet<ProductCategory> ProductCategories { get; set; }
        public virtual IDbSet<Products> Products { get; set; }

        public MgmSysDbContext()
            : base("MgmUser")
        {

        }

        public MgmSysDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //}
    }
}

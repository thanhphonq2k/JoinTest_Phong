using System.Data.Entity;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using Mgm.EntityFramework.MgmSys;

namespace Mgm
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(MgmCoreModule))]
    public class MgmDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "MgmUser";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Database.SetInitializer<MgmSysDbContext>(null);
        }
    }
}

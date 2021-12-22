using System.Reflection;
using Abp.Modules;

namespace Mgm
{
    [DependsOn(typeof(MgmCoreModule))]
    public class MgmApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

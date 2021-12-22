using System.Reflection;
using Abp.Modules;

namespace Mgm
{
    public class MgmCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

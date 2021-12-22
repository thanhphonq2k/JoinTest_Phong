using System.Reflection;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;
using Mgm.Authentication;

namespace Mgm
{
    [DependsOn(typeof(AbpWebApiModule), typeof(MgmApplicationModule))]
    public class MgmWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(MgmApplicationModule).Assembly, "app")
                .Build();

            //Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new AuthenticationFilter());
        }
    }
}

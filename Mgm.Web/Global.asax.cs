using System;
using System.Threading;
using Abp.Castle.Logging.Log4Net;
using Abp.Web;
using Castle.Facilities.Logging;

namespace Mgm.Web
{
    public class MvcApplication : AbpWebApplication<MgmWebModule>
    {
        public Thread breakAndLunchJob = null;

        protected override void Application_Start(object sender, EventArgs e)
        {
            AbpBootstrapper.IocManager.IocContainer
                .AddFacility<LoggingFacility>(f => f.UseAbpLog4Net()
                    .WithConfig(Server.MapPath("log4net.config"))
                );

            base.Application_Start(sender, e);
        }
    }
}

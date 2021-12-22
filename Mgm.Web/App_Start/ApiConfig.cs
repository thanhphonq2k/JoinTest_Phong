using Mgm.Web.Config;
using System.Web.Http;

namespace Mgm.Web.App_Start
{
    public class ApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.SetCorsPolicyProviderFactory(new CorsPolicyFactory());
            config.EnableCors();
        }
    }
}
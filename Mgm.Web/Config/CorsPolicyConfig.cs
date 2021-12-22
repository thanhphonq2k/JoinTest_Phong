using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace Mgm.Web.Config
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class CorsPolicyConfig : Attribute, ICorsPolicyProvider
    {
        private CorsPolicy _policy;

        public CorsPolicyConfig()
        {
            // Create a CORS policy.
            _policy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true,
                AllowAnyOrigin = true
            };
            /*
            // Add allowed origins.
            string[] corsOrigins = System.Configuration.ConfigurationManager.AppSettings["CorsOrigins"].Split(';');
            for (int i = 0; i < corsOrigins.Length; i++)
            {
                var origin = corsOrigins[i];
                if (!string.IsNullOrEmpty(origin))
                {
                    _policy.Origins.Add(origin);
                }
            }
            */
        }

        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_policy);
        }
    }
}
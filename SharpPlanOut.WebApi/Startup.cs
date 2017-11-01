using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Newtonsoft.Json;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using SharpPlanOut.Di;
using SharpPlanOut.WebApi;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;

[assembly: OwinStartup(typeof(Startup))]

namespace SharpPlanOut.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            var cors = GetCorsPolicy();
            app.UseCors(new CorsOptions()
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = request => Task.FromResult(cors)
                }
            });

            WebApiConfig.Register(config);
            app.UseNinjectMiddleware(InversionOfControlContainer.GetInversionOfControlContainer().CreateKernel).UseNinjectWebApi(config);
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        private CorsPolicy GetCorsPolicy()
        {
            var policy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true,
            };
            var corsOrigins = ConfigurationManager.AppSettings["CorsOrigins"];
            if (string.IsNullOrEmpty(corsOrigins)) return policy;

            foreach (var origin in corsOrigins.Split(';'))
            {
                policy.Origins.Add(origin);
            }
            return policy;
        }
    }
}
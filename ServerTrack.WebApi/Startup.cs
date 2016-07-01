using System.Net;
using System.Reflection;
using System.Web.Http;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using Bootstrapper = ServerTrack.WebApi.Utilities.Bootstrapper;

namespace ServerTrack.WebApi
{
    public class Startup
    {
        public static HttpConfiguration HttpConfiguration;

        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.DependencyResolver = new NinjectDependencyResolver(CreateKernel());

            config.Routes.MapHttpRoute("default", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            appBuilder.UseWebApi(config);
            WebApiConfig.Register(config);
        }

        /// <summary>
        /// Builds the kernel.  Virtual and protected to allow extending for testing.
        /// </summary>
        /// <returns></returns>
        protected virtual StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            Bootstrapper.Configure(kernel);
            return kernel;
        }
    }
}
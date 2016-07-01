using System.Net;
using System.Reflection;
using System.Web.Http;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;

namespace ServerTrack.WebApi.SelfHost
{

    public class Startup
    {
        public static HttpConfiguration HttpConfiguration;

        public void Configuration(IAppBuilder appBuilder)
        {
            var listener = (HttpListener)appBuilder.Properties["System.Net.HttpListener"];
            listener.AuthenticationSchemes = AuthenticationSchemes.IntegratedWindowsAuthentication;
            var webApiConfiguration = new HttpConfiguration();
            appBuilder.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(webApiConfiguration);
            WebApiConfig.Register(webApiConfiguration);
        }

        /// <summary>
        /// Builds the kernel.  Virtual and protected to allow extending for testing.
        /// </summary>
        /// <returns></returns>
        protected virtual StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            Utilities.Bootstrapper.Configure(kernel);
            return kernel;
        }
    }
}
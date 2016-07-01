using Ninject;
using ServerTrack.WebApi.Repositories;

namespace ServerTrack.WebApi.Utilities
{
    public static class Bootstrapper
    {
        public static void Configure(IKernel kernel)
        {
            kernel.Bind<ServerLoadRepository>()
               .ToConstant(new ServerLoadRepository())
               .InSingletonScope();
        }
    }
}
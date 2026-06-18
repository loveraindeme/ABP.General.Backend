using General.Backend.HttpApi.Client;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace General.DynamicClient.HttpApi.Client
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(GeneralHttpApiClientModule)
        )]
    public class GeneralDynamicClientHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<AbpRemoteServiceOptions>(options =>
            {
                options.RemoteServices.Add(
                    GeneralHttpApiClientModule.remoteServiceName, 
                    new RemoteServiceConfiguration("http://localhost:5025/"));
            });
        }
    }
}

using General.Backend.HttpApi.StaticClient.WithContracts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace General.StaticClient.WithContracts.HttpApi.Client
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(GeneralHttpApiClientWithContractsModule)
        )]
    public class GeneralStaticClientWithContractsHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<AbpRemoteServiceOptions>(options =>
            {
                options.RemoteServices.Add(
                    GeneralHttpApiClientWithContractsModule.RemoteServiceName,
                    new RemoteServiceConfiguration("http://localhost:5025/"));
            });
        }
    }
}

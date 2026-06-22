using General.Backend.HttpApi.StaticClient.WithoutContracts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace General.StaticClient.WithoutContracts.HttpApi.Client
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(GeneralHttpApiClientWithoutContractsModule)
        )]
    public class GeneralStaticClientWithoutContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<AbpRemoteServiceOptions>(options =>
            {
                options.RemoteServices.Add(
                    GeneralHttpApiClientWithoutContractsModule.RemoteServiceName,
                    new RemoteServiceConfiguration("http://localhost:5025/"));
            });
        }
    }
}

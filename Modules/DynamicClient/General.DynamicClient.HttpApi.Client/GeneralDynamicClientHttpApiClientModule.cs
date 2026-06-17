using General.Backend.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace General.DynamicClient.HttpApi.Client
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(GeneralApplicationContractsModule)
        )]
    public class GeneralDynamicClientHttpApiClientModule : AbpModule
    {
        private const string remoteServiceName = "GeneralBackend";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(GeneralApplicationContractsModule).Assembly, 
                remoteServiceConfigurationName: remoteServiceName
            );
        }
    }
}

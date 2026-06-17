using General.Backend.HttpApi.Client;
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

    }
}

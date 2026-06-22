using General.Backend.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace General.Backend.HttpApi.StaticClient.WithoutContracts
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(AbpVirtualFileSystemModule),
        typeof(GeneralApplicationContractsModule)
    )]
    public class GeneralHttpApiClientWithoutContractsModule : AbpModule
    {
        public const string RemoteServiceName = "GeneralBackend";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddStaticHttpClientProxies(
                typeof(GeneralApplicationContractsModule).Assembly,
                RemoteServiceName
            );

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GeneralHttpApiClientWithoutContractsModule>();

                var assemblyDirectory = Path.GetDirectoryName(
                    typeof(GeneralHttpApiClientWithoutContractsModule).Assembly.Location)!;
                options.FileSets.AddPhysical(Path.Combine(assemblyDirectory, "ClientProxies"));
            });
        }
    }
}

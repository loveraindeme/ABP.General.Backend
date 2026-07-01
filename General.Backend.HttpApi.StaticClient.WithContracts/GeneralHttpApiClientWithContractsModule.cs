using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace General.Backend.HttpApi.StaticClient.WithContracts
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(AbpVirtualFileSystemModule)
    )]
    public class GeneralHttpApiClientWithContractsModule : AbpModule
    {
        public const string RemoteServiceName = "GeneralBackend";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddStaticHttpClientProxies(
                typeof(GeneralHttpApiClientWithContractsModule).Assembly,
                RemoteServiceName
            );

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GeneralHttpApiClientWithContractsModule>();

                var assemblyDirectory = Path.GetDirectoryName(
                    typeof(GeneralHttpApiClientWithContractsModule).Assembly.Location)!;
                options.FileSets.AddPhysical(Path.Combine(assemblyDirectory, "ClientProxies"));
            });
        }
    }
}

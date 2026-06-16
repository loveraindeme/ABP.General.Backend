using General.InformationCollection.Domain.IRepositories;
using General.InformationCollection.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using GeneralModule = General.InformationCollection.Domain.Entities;
namespace General.InformationCollection.Domain
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(GeneralInformationCollectionDomainSharedModule)
    )]
    public class GeneralInformationCollectionDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<InformationCollectionOptions>(
                configuration.GetSection(InformationCollectionOptions.InformationCollectionOption));

            if (context.Services.IsDataMigrationEnvironment())
            {
                Configure<InformationCollectionOptions>(options =>
                {
                    options.IsEnabled = false;
                });
            }
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
        }

        public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            await CollectModuleInformationAsync(context);
        }

        private async Task CollectModuleInformationAsync(ApplicationInitializationContext context)
        {
            var InformationCollectionOption = context
                .ServiceProvider
                .GetRequiredService<IOptions<InformationCollectionOptions>>()
                .Value;

            if (!InformationCollectionOption.IsEnabled)
            {
                return;
            }

            string moduleName = !string.IsNullOrWhiteSpace(InformationCollectionOption.ModuleName)
                ? InformationCollectionOption.ModuleName
                : Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown";
            var compiledVersion = string.Empty;
            var compiledTime = string.Empty;
            var attribute = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (attribute?.InformationalVersion != null)
            {
                var softwareInfo = attribute.InformationalVersion;
                var addIndex = softwareInfo.IndexOf('+');
                if (addIndex > 0)
                {
                    compiledVersion = $"V{softwareInfo[..addIndex]}";
                    compiledTime = softwareInfo.Substring(addIndex + 1, softwareInfo.Length - addIndex - 1);
                }
            }
            var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();
            await Task.Run(async () =>
            {
                using var scope = rootServiceProvider.CreateScope();
                var moduleRepository = scope.ServiceProvider.GetRequiredService<IModuleRepository>();
                var currentModule = await moduleRepository.FindAsync(x => x.Name == moduleName);
                if (currentModule != null)
                {
                    if (currentModule.CompiledVersion != compiledVersion || currentModule.CompiledTime != compiledTime)
                    {
                        currentModule.SetCompiledVersion(compiledVersion);
                        currentModule.SetCompiledTime(compiledTime);
                        await moduleRepository.UpdateAsync(currentModule);
                    }
                }
                else
                {
                    currentModule = new GeneralModule.Module(
                        moduleName,
                        compiledVersion,
                        compiledTime
                    );
                    await moduleRepository.InsertAsync(currentModule);
                }
            });
        }
    }
}

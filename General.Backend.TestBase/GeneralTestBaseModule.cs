using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp;
using General.Backend.Domain;
using Volo.Abp.Threading;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;

namespace General.Backend.TestBase
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpTestBaseModule),
        typeof(GeneralDomainModule)
        )]
    public class GeneralTestBaseModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            AsyncHelper.RunSync(async () =>
            {
                using var scope = context.ServiceProvider.CreateScope();
                await scope.ServiceProvider
                    .GetRequiredService<IDataSeeder>()
                    .SeedAsync();
            });
        }
    }
}

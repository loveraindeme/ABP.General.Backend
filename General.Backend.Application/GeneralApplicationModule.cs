using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace General.Backend.Application
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(GeneralDomainModule),
        typeof(GeneralApplicationContractsModule)
        )]
    public class GeneralApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<GeneralApplicationModule>();
            });
        }
    }
}

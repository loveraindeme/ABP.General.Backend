using General.InformationCollection.Domain;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace General.InformationCollection.Application
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(GeneralInformationCollectionApplicationContractsModule),
        typeof(GeneralInformationCollectionDomainModule)
    )]
    public class GeneralInformationCollectionApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<GeneralInformationCollectionApplicationModule>();
            });
        }
    }
}

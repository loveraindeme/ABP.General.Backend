using General.InformationCollection.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace General.Backend.Application
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(GeneralDomainModule),
        typeof(GeneralApplicationContractsModule),
        typeof(GeneralInformationCollectionApplicationModule),
        typeof(AbpSettingManagementApplicationModule)
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

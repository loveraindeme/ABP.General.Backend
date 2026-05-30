using General.Backend.Domain.Shared;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace General.Backend.Domain
{
    [DependsOn(
        typeof(GeneralDomainSharedModule),
        typeof(AbpSettingManagementDomainModule)
    )]
    public class GeneralDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<SettingManagementOptions>(options =>
            {
                //options.SaveStaticSettingsToDatabase = false;
            });
        }
    }
}

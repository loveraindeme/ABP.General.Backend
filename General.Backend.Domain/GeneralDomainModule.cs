using General.Backend.Domain.Settings;
using General.Backend.Domain.Shared;
using General.Ftp;
using General.Ftp.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace General.Backend.Domain
{
    [DependsOn(
        typeof(GeneralDomainSharedModule),
        typeof(AbpSettingManagementDomainModule),
        typeof(GeneralFtpModule)
    )]
    public class GeneralDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<IFtpConnectionOptionsProvider, SystemSettingFtpConnectionOptionsProvider>();

            Configure<SettingManagementOptions>(options =>
            {
                //options.SaveStaticSettingsToDatabase = false;
            });
        }
    }
}

using General.Ftp.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace General.Ftp
{
    public class GeneralFtpModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<AppSettingsFtpConnectionOptionsProvider>();
            context.Services.AddTransient<FtpFileServiceBuilder, DefaultFtpFileServiceBuilder>();
        }
    }
}

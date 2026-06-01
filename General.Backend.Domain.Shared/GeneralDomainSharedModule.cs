using General.Backend.Domain.Shared.Localization;
using System.Reflection;
using System.Text;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Security;
using Volo.Abp.Security.Encryption;
using Volo.Abp.VirtualFileSystem;

namespace General.Backend.Domain.Shared
{
    [DependsOn(
        typeof(AbpLocalizationModule),
        typeof(AbpSecurityModule)
        )]
    public class GeneralDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GeneralDomainSharedModule>(
                    baseNamespace: Assembly.GetExecutingAssembly().GetName().Name);
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<GeneralBackendResource>("zh-CN")
                    .AddVirtualJson("/Localization/Resources");
                options.DefaultResourceType = typeof(GeneralBackendResource);
                options.Languages.Add(new LanguageInfo("zh-CN", "zh-CN", "简体中文"));
                options.Languages.Add(new LanguageInfo("en-US", "en-US", "English"));
            });

            Configure<AbpStringEncryptionOptions>(opts =>
            {
                opts.DefaultPassPhrase = "0GeneralBackend0";
                opts.DefaultSalt = Encoding.UTF8.GetBytes("CaiBuDao");
                opts.InitVectorBytes = Encoding.UTF8.GetBytes("0GeneralBackend0");
            });
        }
    }
}

using General.Backend.Domain.Shared.Localization;
using System.Reflection;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace General.Backend.Domain.Shared
{
    [DependsOn(
        typeof(AbpLocalizationModule)
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
        }
    }
}

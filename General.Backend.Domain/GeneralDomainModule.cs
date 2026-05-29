using General.Backend.Domain.Shared;
using Volo.Abp.Modularity;

namespace General.Backend.Domain
{
    [DependsOn(
        typeof(GeneralDomainSharedModule)
    )]
    public class GeneralDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            
        }
    }
}

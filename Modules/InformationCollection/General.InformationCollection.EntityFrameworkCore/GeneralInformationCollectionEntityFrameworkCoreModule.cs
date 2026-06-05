using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace General.InformationCollection.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class GeneralInformationCollectionEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<GeneralInformationCollectionDbContext>(options =>
            {
                options.AddDefaultRepositories<IGeneralInformationCollectionDbContext>(includeAllEntities: true);
            });
        }
    }
}

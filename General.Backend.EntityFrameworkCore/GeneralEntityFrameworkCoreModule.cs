using Volo.Abp.Modularity;

namespace General.Backend.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreMySQLModule)
        )]
    public class GeneralEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<GeneralDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseMySQL();
            });
        }
    }
}

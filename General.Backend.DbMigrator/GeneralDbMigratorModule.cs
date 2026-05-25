using General.Backend.Domain;
using General.Backend.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace General.Backend.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(GeneralDomainModule),
        typeof(GeneralEntityFrameworkCoreModule))]
    public class GeneralDbMigratorModule : AbpModule
    {

    }
}

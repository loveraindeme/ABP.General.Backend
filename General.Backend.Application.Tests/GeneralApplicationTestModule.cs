using General.Backend.EntityFrameworkCore.Tests;
using Volo.Abp.Modularity;

namespace General.Backend.Application.Tests
{
    [DependsOn(
        typeof(GeneralEntityFrameworkCoreTestModule),
        typeof(GeneralApplicationModule))]
    public class GeneralApplicationTestModule : AbpModule
    {

    }
}

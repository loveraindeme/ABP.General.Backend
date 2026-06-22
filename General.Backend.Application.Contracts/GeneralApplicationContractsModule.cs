using General.InformationCollection.Application.Contracts;
using Volo.Abp.Modularity;

namespace General.Backend.Application.Contracts
{
    [DependsOn(
        typeof(GeneralInformationCollectionApplicationContractsModule)
        )]
    public class GeneralApplicationContractsModule : AbpModule
    {

    }
}

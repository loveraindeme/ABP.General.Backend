using General.InformationCollection.Domain.Entities;
using General.InformationCollection.Domain.IRepositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace General.InformationCollection.EntityFrameworkCore.Repositories
{
    public class ModuleRepository : EfCoreRepository<IGeneralInformationCollectionDbContext, Module, Guid>, IModuleRepository
    {
        public ModuleRepository(IDbContextProvider<IGeneralInformationCollectionDbContext> dbContextProvider) : base(dbContextProvider) 
        {
        
        }
    }
}

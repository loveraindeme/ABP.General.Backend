using General.InformationCollection.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace General.InformationCollection.EntityFrameworkCore
{
    [ConnectionStringName("InformationCollection")]
    public interface IGeneralInformationCollectionDbContext : IEfCoreDbContext
    {
        DbSet<Module> Modules { get; }
    }
}
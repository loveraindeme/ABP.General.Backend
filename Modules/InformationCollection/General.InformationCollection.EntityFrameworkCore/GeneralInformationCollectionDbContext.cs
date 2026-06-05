using General.InformationCollection.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace General.InformationCollection.EntityFrameworkCore
{
    [ConnectionStringName("InformationCollection")]
    public class GeneralInformationCollectionDbContext : AbpDbContext<GeneralInformationCollectionDbContext>, IGeneralInformationCollectionDbContext
    {
        public DbSet<Module> Modules { get; set; }

        public GeneralInformationCollectionDbContext(DbContextOptions<GeneralInformationCollectionDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ConfigureInformationCollection();
        }
    }
}

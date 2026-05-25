using General.Backend.Domain.DataSeed;
using Volo.Abp.DependencyInjection;

namespace General.Backend.EntityFrameworkCore
{
    public class GeneralDbSchemaMigrator : IGeneralDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public GeneralDbSchemaMigrator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            await _serviceProvider
                .GetRequiredService<GeneralDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}

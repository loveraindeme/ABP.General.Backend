using General.Backend.Domain.DataSeed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;

namespace General.Backend.DbMigrator
{
    public class DbMigratorHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IConfiguration _configuration;

        public DbMigratorHostedService(
            IHostApplicationLifetime hostApplicationLifetime,
            IConfiguration configuration)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var application = await AbpApplicationFactory.CreateAsync<GeneralDbMigratorModule>(options =>
            {
                options.Services.ReplaceConfiguration(_configuration);
                options.UseAutofac();
            }))
            {
                await application.InitializeAsync();

                await application
                    .ServiceProvider
                    .GetRequiredService<GeneralDbMigrationService>()
                    .MigrateAsync();

                await application.ShutdownAsync();

                _hostApplicationLifetime.StopApplication();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

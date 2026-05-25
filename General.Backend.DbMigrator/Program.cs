using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace General.Backend.DbMigrator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<DbMigratorHostedService>();
                });
            return hostBuilder;
        }
    }
}

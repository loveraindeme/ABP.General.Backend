using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace General.Backend.EntityFrameworkCore
{
    public class GeneralDbContextFactory : IDesignTimeDbContextFactory<GeneralDbContext>
    {
        public GeneralDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();
            var builder = new DbContextOptionsBuilder<GeneralDbContext>()
                .UseMySql(
                    connectionString: configuration.GetConnectionString("Default"),
                    serverVersion: MySqlServerVersion.LatestSupportedServerVersion,
                    mySqlOptionsAction: optionsBuilder => optionsBuilder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName
                ));

            return new GeneralDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../General.Backend.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}

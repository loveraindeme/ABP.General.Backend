using General.Backend.TestBase;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace General.Backend.EntityFrameworkCore.Tests
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreSqliteModule),
        typeof(GeneralTestBaseModule),
        typeof(GeneralEntityFrameworkCoreModule)
        )]
    public class GeneralEntityFrameworkCoreTestModule : AbpModule
    {
        private SqliteConnection? _sqliteConnection;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            _sqliteConnection = CreateDatabaseAndGetConnection();
            Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(config =>
                {
                    config.DbContextOptions.UseSqlite(_sqliteConnection);
                });
            });

        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            _sqliteConnection?.Dispose();
        }

        public static SqliteConnection CreateDatabaseAndGetConnection()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<GeneralDbContext>()
                .UseSqlite(connection)
                .Options;
            using var context = new GeneralDbContext(options);
            context.GetService<IRelationalDatabaseCreator>().CreateTables();
            return connection;
        }
    }
}

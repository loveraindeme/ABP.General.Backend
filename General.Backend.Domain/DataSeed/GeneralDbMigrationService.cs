using System.Diagnostics;
using System.Runtime.InteropServices;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace General.Backend.Domain.DataSeed
{
    public class GeneralDbMigrationService : ITransientDependency
    {
        private readonly IDataSeeder _dataSeeder;
        private readonly IEnumerable<IGeneralDbSchemaMigrator> _generalDbSchemaMigrators;

        public GeneralDbMigrationService(
            IDataSeeder dataSeeder,
            IEnumerable<IGeneralDbSchemaMigrator> generalDbSchemaMigrators)
        {
            _dataSeeder = dataSeeder;
            _generalDbSchemaMigrators = generalDbSchemaMigrators;
        }

        public async Task MigrateAsync()
        {
            var initialMigrationAdded = AddInitialMigrationIfNotExist();
            if (initialMigrationAdded)
            {
                return;
            }

            foreach (var migrator in _generalDbSchemaMigrators)
            {
                await migrator.MigrateAsync();
            }
            await _dataSeeder.SeedAsync();
        }

        private static bool AddInitialMigrationIfNotExist()
        {
            try
            {
                if (!DbMigrationsProjectExists())
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            try
            {
                if (!MigrationsFolderExists())
                {
                    AddInitialMigration();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool DbMigrationsProjectExists()
        {
            var dbMigrationsProjectFolder = GetEntityFrameworkCoreProjectFolderPath();
            return dbMigrationsProjectFolder != null;
        }

        private static bool MigrationsFolderExists()
        {
            var dbMigrationsProjectFolder = GetEntityFrameworkCoreProjectFolderPath();
            return dbMigrationsProjectFolder != null && Directory.Exists(Path.Combine(dbMigrationsProjectFolder, "Migrations"));
        }

        private static void AddInitialMigration()
        {
            string argumentPrefix;
            string fileName;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                argumentPrefix = "-c";
                fileName = "/bin/bash";
            }
            else
            {
                argumentPrefix = "/C";
                fileName = "cmd.exe";
            }
            var procStartInfo = new ProcessStartInfo(fileName,
                $"{argumentPrefix} \"abp create-migration-and-run-migrator \"{GetEntityFrameworkCoreProjectFolderPath()}\"\""
            );
            try
            {
                Process.Start(procStartInfo);
            }
            catch (Exception)
            {
                throw new Exception("无法运行ABP命令行");
            }
        }

        private static string? GetEntityFrameworkCoreProjectFolderPath()
        {
            var slnDirectoryPath = GetSolutionDirectoryPath();
            return slnDirectoryPath == null
                ? throw new Exception("未找到解决方案文件夹")
                : Directory.GetDirectories(slnDirectoryPath).FirstOrDefault(d => d.EndsWith(".EntityFrameworkCore"));
        }

        private static string? GetSolutionDirectoryPath()
        {
            var currentDirectory = new DirectoryInfo(AppContext.BaseDirectory);
            while (currentDirectory != null && Directory.GetParent(currentDirectory.FullName) != null)
            {
                currentDirectory = Directory.GetParent(currentDirectory.FullName);
                if (currentDirectory != null && Directory.GetFiles(currentDirectory.FullName).FirstOrDefault(f => f.EndsWith(".sln")) != null)
                {
                    return currentDirectory.FullName;
                }
            }
            return null;
        }
    }
}

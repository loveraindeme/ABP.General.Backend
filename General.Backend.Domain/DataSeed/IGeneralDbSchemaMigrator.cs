namespace General.Backend.Domain.DataSeed
{
    public interface IGeneralDbSchemaMigrator
    {
        public Task MigrateAsync();
    }
}

using General.Backend.EntityFrameworkCore.Mappings.Extensions;
using System.Reflection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace General.Backend.EntityFrameworkCore
{
    [ReplaceDbContext(typeof(ISettingManagementDbContext))]
    [ConnectionStringName("Default")]
    public class GeneralDbContext : AbpDbContext<GeneralDbContext>, ISettingManagementDbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<RoleMenu> RoleMenus { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<SettingDefinitionRecord> SettingDefinitionRecords { get; set; }

        public GeneralDbContext(DbContextOptions<GeneralDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ConfigureSystemSetting();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

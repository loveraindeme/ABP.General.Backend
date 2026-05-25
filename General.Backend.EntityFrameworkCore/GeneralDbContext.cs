using System.Reflection;

namespace General.Backend.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class GeneralDbContext : AbpDbContext<GeneralDbContext>
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<RoleMenu> RoleMenus { get; set; }

        public GeneralDbContext(DbContextOptions<GeneralDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

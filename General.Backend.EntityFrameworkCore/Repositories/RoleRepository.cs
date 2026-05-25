using General.Backend.EntityFrameworkCore.Repositories.Extensions;

namespace General.Backend.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// 角色仓储
    /// </summary>
    public class RoleRepository : BaseRepository<Role, Guid>, IRoleRepository
    {
        public RoleRepository(IDbContextProvider<GeneralDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public override async Task<IQueryable<Role>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }

        public async Task<List<Role>> GetSameRoleAsync(SameRoleSpecification specification, CancellationToken cancellationToken = default)
        {
            var roles = await GetDbSetAsync();
            var result = await roles
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
            return result;
        }
    }
}

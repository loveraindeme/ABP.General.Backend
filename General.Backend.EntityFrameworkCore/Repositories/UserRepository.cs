using General.Backend.EntityFrameworkCore.Repositories.Extensions;

namespace General.Backend.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// 用户仓储
    /// </summary>
    public class UserRepository : BaseRepository<User, Guid>, IUserRepository
    {
        public UserRepository(IDbContextProvider<GeneralDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public override async Task<IQueryable<User>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }

        public async Task<User?> FindUserByAccountAsync(string account, bool include = true, CancellationToken cancellationToken = default)
        {
            var users = await GetDbSetAsync();
            var result = await users
                .IncludeIf(include, user => user.UserRoles)
                .FirstOrDefaultAsync(user => user.Account == account, GetCancellationToken(cancellationToken));
            return result;
        }

        public async Task<List<User>> GetSameUserAsync(SameUserSpecification specification, CancellationToken cancellationToken = default)
        {
            var users = await GetDbSetAsync();
            var result = await users
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
            return result;
        }
    }
}

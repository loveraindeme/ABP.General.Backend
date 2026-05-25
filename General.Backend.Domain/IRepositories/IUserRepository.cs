using General.Backend.Domain.Specifications;

namespace General.Backend.Domain.IRepositories
{
    /// <summary>
    /// 用户仓储
    /// </summary>
    public interface IUserRepository : IBaseRepository<User, Guid>
    {
        public Task<User?> FindUserByAccountAsync(string account, bool include = true, CancellationToken cancellationToken = default);

        public Task<List<User>> GetSameUserAsync(SameUserSpecification specification, CancellationToken cancellationToken = default);

    }
}

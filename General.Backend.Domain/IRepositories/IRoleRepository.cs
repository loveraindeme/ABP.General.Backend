namespace General.Backend.Domain.IRepositories
{
    /// <summary>
    /// 角色仓储
    /// </summary>
    public interface IRoleRepository : IBaseRepository<Role, Guid>
    {
        public Task<List<Role>> GetSameRoleAsync(SameRoleSpecification specification, CancellationToken cancellationToken = default);
    }
}

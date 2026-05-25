namespace General.Backend.EntityFrameworkCore.Repositories.Extensions
{
    public static class RoleRepositoryExtensions
    {
        public static IQueryable<Role> IncludeDetails(
            this IQueryable<Role> queryable)
        {
            return queryable.Include(role => role.RoleMenus);
        }
    }
}

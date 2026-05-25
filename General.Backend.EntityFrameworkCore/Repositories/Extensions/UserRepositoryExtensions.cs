namespace General.Backend.EntityFrameworkCore.Repositories.Extensions
{
    public static class UserRepositoryExtensions
    {
        public static IQueryable<User> IncludeDetails(
            this IQueryable<User> queryable)
        {
            return queryable.Include(user => user.UserRoles);
        }
    }
}

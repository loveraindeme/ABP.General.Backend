namespace General.Backend.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// 菜单仓储
    /// </summary>
    public class MenuRepository : BaseRepository<Menu, Guid>, IMenuRepository
    {
        public MenuRepository(IDbContextProvider<GeneralDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
    }
}

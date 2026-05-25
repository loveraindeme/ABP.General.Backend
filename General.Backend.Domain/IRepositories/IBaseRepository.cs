namespace General.Backend.Domain.IRepositories
{
    /// <summary>
    /// 基础仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IBaseRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        public string GenerateSortExpression(string sortField, string sortType, Type type);
    }
}

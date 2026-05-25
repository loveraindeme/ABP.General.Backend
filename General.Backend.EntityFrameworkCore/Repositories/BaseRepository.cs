using System.Reflection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;

namespace General.Backend.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// 基础仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class BaseRepository<TEntity, TKey> : EfCoreRepository<GeneralDbContext, TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        public BaseRepository(IDbContextProvider<GeneralDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public string GenerateSortExpression(string sortField, string sortType, Type type)
        {
            string sortExpression = string.Empty;
            if (IsValidField(sortField, type))
            {
                sortExpression = sortField;
                if (sortType != "asc")
                {
                    sortExpression += " desc";
                }
                else
                {
                    sortExpression += " asc";
                }
            }
            return sortExpression;
        }

        public bool IsValidField(string field, Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name.Equals(field))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

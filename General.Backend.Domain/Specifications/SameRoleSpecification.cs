using System.Linq.Expressions;
using Volo.Abp.Specifications;

namespace General.Backend.Domain.Specifications
{
    public class SameRoleSpecification : Specification<Role>
    {
        /// <summary>
        /// 角色编码
        /// </summary>
        public string Code { get; } = string.Empty;

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; } = string.Empty;

        public SameRoleSpecification(string name, string code = "")
        {
            Code = code;
            Name = name;
        }

        public override Expression<Func<Role, bool>> ToExpression()
        {
            return user => user.Code == Code || user.Name == Name;
        }
    }
}

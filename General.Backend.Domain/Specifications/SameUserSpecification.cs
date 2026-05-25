using System.Linq.Expressions;
using Volo.Abp.Specifications;

namespace General.Backend.Domain.Specifications
{
    public class SameUserSpecification : Specification<User>
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; } = string.Empty;

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; } = string.Empty;

        public SameUserSpecification(string userName, string account = "")
        {
            Account = account;
            UserName = userName;
        }

        public override Expression<Func<User, bool>> ToExpression()
        {
            return user => user.Account == Account || user.Name == UserName;
        }
    }
}

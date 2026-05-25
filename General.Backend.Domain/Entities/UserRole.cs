namespace General.Backend.Domain.Entities
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class UserRole : Entity<Guid>, IHasCreationTime
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual Guid UserId { get; private set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public virtual Guid RoleId { get; private set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; private set; }

        protected UserRole()
        {

        }

        internal UserRole(
            Guid id,
            Guid userId,
            Guid roleId)
        {
            Id = id;
            UserId = userId;
            RoleId = roleId;
        }
    }
}

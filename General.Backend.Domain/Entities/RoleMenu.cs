namespace General.Backend.Domain.Entities
{
    /// <summary>
    /// 角色菜单
    /// </summary>
    public class RoleMenu : Entity<Guid>, IHasCreationTime
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public virtual Guid RoleId { get; private set; }

        /// <summary>
        /// 菜单编码
        /// </summary>
        public virtual string MenuCode { get; private set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; private set; }

        protected RoleMenu()
        {

        }

        internal RoleMenu(
            Guid id,
            Guid roleId,
            [NotNull] string menuCode)
        {
            Id = id;
            RoleId = roleId;
            MenuCode = Check.Length(menuCode, nameof(menuCode), RoleConsts.MaxMenuCodeLength, RoleConsts.MinMenuCodeLength)!;
        }
    }
}

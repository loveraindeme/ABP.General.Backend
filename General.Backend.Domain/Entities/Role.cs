namespace General.Backend.Domain.Entities
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; private set; } = string.Empty;

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; private set; } = string.Empty;

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string? Remark { get; set; }

        /// <summary>
        /// 角色关联菜单列表
        /// </summary>
        public virtual ICollection<RoleMenu> RoleMenus { get; private set; } = [];


        protected Role()
        {

        }

        public Role(
            Guid id,
            [NotNull] string code,
            [NotNull] string name,
            [CanBeNull] string? remark = null)
        {
            Id = id;
            Code = Check.Length(code, nameof(code), RoleConsts.MaxCodeLength, RoleConsts.MinCodeLength)!;
            SetName(name);
            Remark = Check.Length(remark, nameof(remark), RoleConsts.MaxRemarkLength);
        }

        internal virtual void SetName(
            [NotNull] string name)
        {
            Name = Check.Length(name, nameof(name), UserConsts.MaxNameLength, UserConsts.MinNameLength)!;
        }

        public virtual void SetMenus(ICollection<RoleMenu> roleMenus)
        {
            RoleMenus = roleMenus;
        }
    }
}

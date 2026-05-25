namespace General.Backend.Application.Contracts.Dtos.Roles
{
    /// <summary>
    /// 角色菜单
    /// </summary>
    public class RoleMenuDto : EntityDto<Guid>, IHasCreationTime
    {
        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; } = string.Empty;

        /// <summary>
        /// 菜单编码
        /// </summary>
        public string MenuCode { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; }
    }
}

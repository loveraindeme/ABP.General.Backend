namespace General.Backend.Application.Contracts.Dtos.Menus
{
    /// <summary>
    /// 权限更新
    /// </summary>
    public class PermissionUpdateDto
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 菜单模块列表
        /// </summary>
        public List<string> MenuModules { get; set; } = new List<string>();
    }
}

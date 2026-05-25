namespace General.Backend.Application.Contracts.Dtos.Menus
{
    /// <summary>
    /// 权限
    /// </summary>
    public class PermissionDto
    {
        /// <summary>
        /// 菜单树
        /// </summary>
        public List<MenuModuleDto> MenuModule { get; set; } = [];

        /// <summary>
        /// 选中的菜单
        /// </summary>
        public List<string> CheckedMenu { get; set; } = [];

        /// <summary>
        /// 选中的模块
        /// </summary>
        public List<string> CheckedModule { get; set; } = [];
    }
}

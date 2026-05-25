namespace General.Backend.Application.Contracts.Dtos.Menus
{
    /// <summary>
    /// 菜单模块
    /// </summary>
    public class MenuModuleDto
    {
        /// <summary>
        /// 父编码
        /// </summary>
        public string ParentCode { get; set; } = string.Empty;

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; } = string.Empty;
    }
}

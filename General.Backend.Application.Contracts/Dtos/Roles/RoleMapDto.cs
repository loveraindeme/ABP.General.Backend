namespace General.Backend.Application.Contracts.Dtos.Roles
{
    /// <summary>
    /// 角色列表
    /// </summary>
    public class RoleMapDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 角色编码
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}

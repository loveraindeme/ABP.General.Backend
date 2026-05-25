namespace General.Backend.Application.Contracts.Dtos.Roles
{
    /// <summary>
    /// 角色查询
    /// </summary>
    public class RoleQueryDto : PageAndSortDto
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string? Name { get; set; } = string.Empty;
    }
}

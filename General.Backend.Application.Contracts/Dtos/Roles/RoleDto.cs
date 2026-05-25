namespace General.Backend.Application.Contracts.Dtos.Roles
{
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }
    }
}

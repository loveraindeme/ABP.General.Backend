namespace General.Backend.Application.Contracts.Dtos.Roles
{
    /// <summary>
    /// 角色更新
    /// </summary>
    public class RoleUpdateDto : EntityDto<Guid>
    {
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

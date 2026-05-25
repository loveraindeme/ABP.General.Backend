
namespace General.Backend.Application.Contracts.Dtos.Users
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 联系方式
        /// </summary>
        public string? Contact { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// 是否冻结（0：未冻结，1：已冻结）
        /// </summary>
        public FrozenStatus IsFrozen { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public List<Guid> RoleIds { get; set; } = [];

        /// <summary>
        /// 角色名称
        /// </summary>
        public List<string> RoleNames { get; set; } = [];
    }
}

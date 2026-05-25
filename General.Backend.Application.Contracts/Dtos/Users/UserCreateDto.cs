namespace General.Backend.Application.Contracts.Dtos.Users
{
    /// <summary>
    /// 用户创建
    /// </summary>
    public class UserCreateDto
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
        /// 用户关联角色列表
        /// </summary>
        public List<Guid> RoleIds { get; set; } = [];
    }
}

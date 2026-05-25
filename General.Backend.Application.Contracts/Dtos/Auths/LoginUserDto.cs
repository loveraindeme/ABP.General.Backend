namespace General.Backend.Application.Contracts.Dtos.Auths
{
    /// <summary>
    /// 登录用户
    /// </summary>
    public class LoginUserDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAccount { get; set; } = string.Empty;

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; } = string.Empty;

        /// <summary>
        /// 访问凭证值
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// 访问凭证有效时间，单位：分钟
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// 访问过期时间
        /// </summary>
        public DateTime ExpiresTime { get; set; }

        /// <summary>
        /// 刷新凭证值
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// 刷新凭证有效时间，单位：分钟
        /// </summary>
        public int RefreshExpiresIn { get; set; }

        /// <summary>
        /// 刷新过期时间
        /// </summary>
        public DateTime RefreshExpiresTime { get; set; }
    }
}

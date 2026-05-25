namespace General.Backend.Application.Contracts.Dtos.Auths
{
    /// <summary>
    /// 令牌
    /// </summary>
    public class AccessTokenDto
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// 访问过期时间
        /// </summary>
        public DateTime ExpiresTime { get; set; }

        /// <summary>
        /// 访问凭证有效时间，单位：分钟
        /// </summary>
        public int ExpiresIn { get; set; }
    }
}

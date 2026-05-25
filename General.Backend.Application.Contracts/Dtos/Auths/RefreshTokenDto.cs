namespace General.Backend.Application.Contracts.Dtos.Auths
{
    /// <summary>
    /// 令牌
    /// </summary>
    public class RefreshTokenDto
    {
        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// 访问过期时间
        /// </summary>
        public DateTime RefreshExpiresTime { get; set; }

        /// <summary>
        /// 访问凭证有效时间，单位：分钟
        /// </summary>
        public int RefreshExpiresIn { get; set; }
    }
}

namespace General.Backend.Application.Options
{
    /// <summary>
    /// JsonWebToken
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// JWT
        /// </summary>
        public const string JwtOption = "JWT";

        /// <summary>
        /// 签发者
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// 收发者
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// 密钥
        /// </summary>
        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// Token有效期（单位：分钟）
        /// </summary>
        public int ExpirationTime { get; set; }

        /// <summary>
        /// Token有效刷新时间（单位：分钟）
        /// </summary>
        public int RefreshTime { get; set; }
    }
}

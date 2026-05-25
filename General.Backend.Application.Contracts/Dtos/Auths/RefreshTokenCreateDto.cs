namespace General.Backend.Application.Contracts.Dtos.Auths
{
    /// <summary>
    /// 刷新令牌创建
    /// </summary>
    public class RefreshTokenCreateDto
    {
        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
    }
}

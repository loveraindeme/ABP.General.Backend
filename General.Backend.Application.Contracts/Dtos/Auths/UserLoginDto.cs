namespace General.Backend.Application.Contracts.Dtos.Auths
{
    /// <summary>
    /// 用户登录
    /// </summary>
    public class UserLoginDto
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAccount { get; set; } = string.Empty;

        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 验证码
        /// </summary>
        public string Captcha { get; set; } = string.Empty;

        /// <summary>
        /// 验证码发放时间时间戳
        /// </summary>
        public string CaptchaTime { get; set; } = string.Empty;
    }
}

namespace General.Backend.Application.Contracts.Dtos.Auths
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class CaptchaDto
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// 验证码文字
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// 验证码图片
        /// </summary>
        public string Img { get; set; } = string.Empty;
    }
}

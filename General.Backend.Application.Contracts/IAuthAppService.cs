using General.Backend.Application.Contracts.Dtos.Auths;

namespace General.Backend.Application.Contracts
{
    /// <summary>
    /// 认证应用服务
    /// </summary>
    public interface IAuthAppService : IApplicationService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<LoginUserDto> LoginAsync(UserLoginDto input);

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public Task<UserPermissionDto> GetUserAsync();

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public Task<CaptchaDto> GetCaptchaAsync();

        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<LoginUserDto> RefreshAsync(RefreshTokenCreateDto input);

    }
}

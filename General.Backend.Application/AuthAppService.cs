using General.Backend.Application.Contracts.Dtos.Auths;
using General.Backend.Application.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace General.Backend.Application
{
    /// <summary>
    /// 认证应用服务
    /// </summary>
    public class AuthAppService : ApplicationService, IAuthAppService
    {
        private readonly ILogger<AuthAppService> _logger;
        private readonly JwtOptions _jwtOptions;
        private readonly ICurrentUser _currentUser;
        private readonly IDistributedCache<string> _captchaCache;
        private readonly IDistributedCache<UserPermissionCacheItem, Guid> _permissionCache;
        private readonly UserService _userService;
        private readonly MenuService _menuService;
        private readonly IUserRepository _userRepository;

        public AuthAppService(
            ILogger<AuthAppService> logger,
            IOptions<JwtOptions> jwtOptions,
            ICurrentUser currentUser,
            IDistributedCache<string> captchaCache,
            IDistributedCache<UserPermissionCacheItem, Guid> permissionCache,
            UserService userService,
            MenuService menuService,
            IUserRepository userRepository)
        {
            _logger = logger;
            _jwtOptions = jwtOptions.Value;
            _currentUser = currentUser;
            _captchaCache = captchaCache;
            _permissionCache = permissionCache;
            _userService = userService;
            _menuService = menuService;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [UnitOfWork(IsDisabled = true)]
        public async Task<LoginUserDto> LoginAsync(UserLoginDto input)
        {
            string? captchaCode = await _captchaCache.GetAsync(input.CaptchaTime);
            if (string.IsNullOrEmpty(captchaCode))
            {
                throw new UserFriendlyException("验证码已过期");
            }
            if (!captchaCode.ToUpper().Equals(input.Captcha.ToUpper()))
            {
                throw new UserFriendlyException("验证码错误");
            }

            var user = await _userService.LoginCheckAsync(input.UserAccount, input.Password);

            var userPermissionCacheItem = await _permissionCache.GetOrAddAsync(
                user.Id,
                async () => await GetUserPermissionAsync(user)
            ) ?? throw new UserFriendlyException("获取登录用户权限异常");
            var decryptAccount = SecurityHelper.AESDecrypt(user.Account);
            var decryptName = SecurityHelper.AESDecrypt(user.Name);
            var userToken = new UserTokenDto
            {
                UserId = user.Id,
                UserAccount = decryptAccount,
                UserName = decryptName,
                RoleCode = userPermissionCacheItem.Role,
                RoleName = userPermissionCacheItem.RoleName
            };
            var accessToken = GetAccessToken(userToken);
            var refreshToken = GetRefreshToken(userToken);

            await _captchaCache.RemoveAsync(input.CaptchaTime);

            return new LoginUserDto
            {
                UserId = user.Id,
                UserAccount = user.Account,
                UserName = user.Name,
                Role = userToken.RoleCode,
                RoleName = userToken.RoleName,
                AccessToken = accessToken.AccessToken,
                ExpiresIn = accessToken.ExpiresIn,
                ExpiresTime = accessToken.ExpiresTime,
                RefreshToken = refreshToken.RefreshToken,
                RefreshExpiresIn = refreshToken.RefreshExpiresIn,
                RefreshExpiresTime = refreshToken.RefreshExpiresTime
            };
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public async Task<UserPermissionDto> GetUserAsync()
        {
            if (!_currentUser.IsAuthenticated)
            {
                throw new UserFriendlyException("获取当前用户权限异常");
            }
            var userPermissionCacheItem = await _permissionCache.GetOrAddAsync(
                _currentUser.Id!.Value,
                async () =>
                {
                    var user = await _userRepository.GetAsync(user => user.Id == _currentUser.Id!.Value);
                    return await GetUserPermissionAsync(user);
                }
            ) ?? throw new UserFriendlyException("获取缓存用户权限异常");
            return ObjectMapper.Map<UserPermissionCacheItem, UserPermissionDto>(userPermissionCacheItem);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<CaptchaDto> GetCaptchaAsync()
        {
            var captchaCode = CaptchaHelper.GenerateRandomCaptcha();
            var dateTimeNow = DateTime.UtcNow;
            var key = dateTimeNow.ToTimestampMillis().ToString();
            await _captchaCache.SetAsync(key, captchaCode, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            return new CaptchaDto
            {
                Key = key,
                Text = captchaCode,
                Img = Convert.ToBase64String(CaptchaHelper.CreateCaptchaImage(captchaCode))
            };
        }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<LoginUserDto> RefreshAsync(RefreshTokenCreateDto input)
        {
            var claim = ValidateToken(
                input.RefreshToken,
                _jwtOptions.Secret,
                _jwtOptions.Audience,
                _jwtOptions.Issuer);

            if (claim.Count <= 2 || !Guid.TryParse(claim[2].Value, out Guid userId))
            {
                throw new UserFriendlyException("无效的令牌");
            }

            var userPermissionCacheItem = await _permissionCache.GetOrAddAsync(
                userId,
                async () =>
                {
                    var user = await _userRepository.GetAsync(user => user.Id == userId);
                    return await GetUserPermissionAsync(user);
                }
            ) ?? throw new UserFriendlyException("获取缓存用户权限异常");

            var decryptAccount = SecurityHelper.AESDecrypt(userPermissionCacheItem.UserAccount);
            var decryptName = SecurityHelper.AESDecrypt(userPermissionCacheItem.UserName);
            var userToken = new UserTokenDto
            {
                UserId = userId,
                UserAccount = decryptAccount,
                UserName = decryptName,
                RoleCode = userPermissionCacheItem.Role,
                RoleName = userPermissionCacheItem.RoleName
            };

            var accessToken = GetAccessToken(userToken);
            var refreshToken = GetRefreshToken(userToken);

            return new LoginUserDto
            {
                UserId = userId,
                UserAccount = userPermissionCacheItem.UserAccount,
                UserName = userPermissionCacheItem.UserName,
                Role = userToken.RoleCode,
                RoleName = userToken.RoleName,
                AccessToken = accessToken.AccessToken,
                ExpiresIn = accessToken.ExpiresIn,
                ExpiresTime = accessToken.ExpiresTime,
                RefreshToken = refreshToken.RefreshToken,
                RefreshExpiresIn = refreshToken.RefreshExpiresIn,
                RefreshExpiresTime = refreshToken.RefreshExpiresTime
            };
        }

        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<UserPermissionCacheItem> GetUserPermissionAsync(User user)
        {
            var userPermissionCacheItem = await _menuService.GetPermissionAsync(user);
            return userPermissionCacheItem;
        }

        /// <summary>
        /// 获取访问令牌
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        private AccessTokenDto GetAccessToken(UserTokenDto userToken)
        {
            string secret = _jwtOptions.Secret;
            var key = Encoding.UTF8.GetBytes(secret);
            var authTime = DateTime.UtcNow; // 授权时间
            var expiresIn = TimeSpan.FromMinutes(_jwtOptions.ExpirationTime);
            DateTime expires = authTime.Add(expiresIn); // 过期时间

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescripor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([
                    new Claim(AuthConsts.Audience, _jwtOptions.Audience),
                    new Claim(AuthConsts.Issuer, _jwtOptions.Issuer),
                    new Claim(AbpClaimTypes.UserId, userToken.UserId.ToString()),
                    new Claim(AuthConsts.UserAccount, userToken.UserAccount),
                    new Claim(AbpClaimTypes.UserName, userToken.UserName),
                    new Claim(AbpClaimTypes.Role, userToken.RoleName),
                    new Claim(AuthConsts.Subject, AuthConsts.ValidateType)
                ]),
                Expires = expires,
                // 对称秘钥SymmetricSecurityKey
                // 签名证书(秘钥，加密算法)SecurityAlgorithms
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescripor);
            var tokenString = tokenHandler.WriteToken(token);
            var accessToken = new AccessTokenDto
            {
                AccessToken = tokenString,
                ExpiresIn = (int)expiresIn.TotalMinutes,
                ExpiresTime = expires,
            };
            return accessToken;
        }

        /// <summary>
        /// 获取刷新令牌
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        private RefreshTokenDto GetRefreshToken(UserTokenDto userToken)
        {
            string secret = _jwtOptions.Secret;
            var key = Encoding.UTF8.GetBytes(secret);
            var authTime = DateTime.UtcNow; // 授权时间
            var expiresIn = TimeSpan.FromMinutes(_jwtOptions.RefreshTime);
            DateTime expires = authTime.Add(expiresIn); // 过期时间

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescripor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([
                    new Claim(AuthConsts.Audience, _jwtOptions.Audience),
                    new Claim(AuthConsts.Issuer, _jwtOptions.Issuer),
                    new Claim(AbpClaimTypes.UserId, userToken.UserId.ToString()),
                    new Claim(AuthConsts.UserAccount, userToken.UserAccount),
                    new Claim(AbpClaimTypes.UserName, userToken.UserName),
                    new Claim(AbpClaimTypes.Role, userToken.RoleName),
                    new Claim(AuthConsts.Subject, AuthConsts.ValidateType)
                ]),
                Expires = expires,
                // 对称秘钥SymmetricSecurityKey
                // 签名证书(秘钥，加密算法)SecurityAlgorithms
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescripor);
            var tokenString = tokenHandler.WriteToken(token);
            var refreshToken = new RefreshTokenDto
            {
                RefreshToken = tokenString,
                RefreshExpiresIn = (int)expiresIn.TotalMinutes,
                RefreshExpiresTime = expires,
            };
            return refreshToken;
        }

        /// <summary>
        /// 校验令牌
        /// </summary>
        /// <param name="token"></param>
        /// <param name="secret"></param>
        /// <param name="audience"></param>
        /// <param name="issuer"></param>
        /// <returns></returns>
        private List<Claim> ValidateToken(string token, string secret, string audience, string issuer)
        {
            var claims = new List<Claim>();
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(secret))
            {
                throw new UserFriendlyException("无效的令牌");
            }
            try
            {
                JwtSecurityToken jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                if (jwtToken == null)
                {
                    throw new UserFriendlyException("无效的令牌");
                }
                //string audience = jwtToken.Claims.ToList()[0].Value;
                //string issuer = jwtToken.Claims.ToList()[1].Value;
                var keyByteArray = Encoding.UTF8.GetBytes(secret);
                new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyByteArray),
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                if (jwtToken.Payload.Claims is List<Claim> claimlist)
                {
                    claims = claimlist;
                }
            }
            catch (SecurityTokenExpiredException ex)
            {
                _logger.LogError(ex, "令牌超时，登录失效");
                throw new UserFriendlyException("令牌超时，登录失效");
            }
            catch (SecurityTokenInvalidLifetimeException ex)
            {
                _logger.LogError(ex, "令牌超时，登录失效");
                throw new UserFriendlyException("令牌超时，登录失效");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "无效的令牌");
                throw new UserFriendlyException("无效的令牌");
            }
            return claims;
        }
    }
}

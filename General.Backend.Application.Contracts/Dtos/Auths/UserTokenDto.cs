namespace General.Backend.Application.Contracts.Dtos.Auths
{
    /// <summary>
    /// 用户令牌
    /// </summary>
    public class UserTokenDto
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
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; } = string.Empty;

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; } = string.Empty;
    }
}

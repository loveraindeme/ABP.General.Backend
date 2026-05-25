namespace General.Backend.Domain.CacheItems
{
    /// <summary>
    /// 用户权限
    /// </summary>
    public class UserPermissionCacheItem
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
        /// 授权访问菜单
        /// </summary>
        public List<MenuRouterCacheItem> MenuRouters { get; set; } = new List<MenuRouterCacheItem>();

        /// <summary>
        /// 授权使用功能
        /// </summary>
        public List<string> Modules { get; set; } = [];
    }
}

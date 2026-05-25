namespace General.Backend.Domain.CacheItems
{
    /// <summary>
    /// 菜单路由
    /// </summary>
    public class MenuRouterCacheItem
    {
        /// <summary>
        /// 路由名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 路由地址
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// 是否隐藏路由，当设置true的时候该路由不会在侧边栏出现
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// 组件地址
        /// </summary>
        public string Component { get; set; } = string.Empty;

        /// <summary>
        /// 重定向地址，当设置noRedirect的时候该路由在面包屑导航中不可被点击
        /// </summary>
        public string Redirect { get; set; } = string.Empty;

        /// <summary>
        /// 当你一个路由下面的children声明的路由大于1个时，自动会变成嵌套的模式--如组件页面
        /// </summary>
        public bool AlwaysShow { get; set; }

        /// <summary>
        /// 菜单内容
        /// </summary>
        public Meta Meta { get; set; }

        /// <summary>
        /// 子菜单路由
        /// </summary>
        public List<MenuRouterCacheItem> Children { get; set; } = [];
    }

    /// <summary>
    /// 菜单内容
    /// </summary>
    public class Meta
    {
        public Meta(string title, string? icon)
        {
            Title = title;
            Icon = icon;
        }

        /// <summary>
        /// 路由在侧边栏和面包屑中展示的名称
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 路由的图标
        /// </summary>
        public string? Icon { get; set; }
    }
}

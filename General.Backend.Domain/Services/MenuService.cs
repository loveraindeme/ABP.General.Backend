namespace General.Backend.Domain.Services
{
    public class MenuService : DomainService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRepository<RoleMenu> _roleMenuRepository;

        public MenuService(
            IMenuRepository menuRepository,
            IRoleRepository roleRepository,
            IRepository<RoleMenu> roleMenuRepository)
        {
            _menuRepository = menuRepository;
            _roleRepository = roleRepository;
            _roleMenuRepository = roleMenuRepository;
        }

        /// <summary>
        /// 获取用户权限缓存
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<UserPermissionCacheItem> GetPermissionAsync(User user)
        {
            var userPermissionCacheItem = new UserPermissionCacheItem
            {
                UserId = user.Id,
                UserAccount = user.Account,
                UserName = user.Name
            };
            var menuModules = new List<Menu>();
            var decryptAccount = SecurityHelper.AESDecrypt(user.Account);
            if (decryptAccount == UserConsts.AdminAccount)
            {
                menuModules = await _menuRepository.GetListAsync();

                userPermissionCacheItem.Role = RoleConsts.AdminRoleCode;
                userPermissionCacheItem.RoleName = RoleConsts.AdminRoleName;
            }
            else
            {
                var roleIds = user.UserRoles.Select(rel => rel.RoleId);
                var roles = await _roleRepository.GetListAsync(role => roleIds.Contains(role.Id));
                var roleMenus = await _roleMenuRepository.GetListAsync(rel => roleIds.Contains(rel.RoleId));
                var menuCodes = roleMenus.Select(rel => rel.MenuCode).Distinct();
                menuModules = await _menuRepository.GetListAsync(menu => menuCodes.Contains(menu.Code));
                userPermissionCacheItem.Role = string.Join(",", roles.Select(x => x.Code).ToList());
                userPermissionCacheItem.RoleName = string.Join(",", roles.Select(x => x.Name).ToList());
            }
            userPermissionCacheItem.MenuRouters = await GetMenuRouter(menuModules);
            foreach (var menuModule in menuModules)
            {
                userPermissionCacheItem.Modules.Add(menuModule.Code);
            }
            return userPermissionCacheItem;
        }

        /// <summary>
        /// 获取路由菜单
        /// </summary>
        /// <param name="menus"></param>
        /// <returns></returns>
        public async Task<List<MenuRouterCacheItem>> GetMenuRouter(List<Menu> menus)
        {
            var menuRouterCacheItems = new List<MenuRouterCacheItem>();
            menus = menus.Where(x => x.Type == MenuConsts.CatalogTypeName || x.Type == MenuConsts.MenuTypeName).OrderBy(y => y.Sort).ToList();
            //获取部分勾选的菜单的目录列表
            var parentCodes = menus.Where(x => x.Type == MenuConsts.MenuTypeName).Select(y => y.ParentCode);
            var exceptCodes = parentCodes.Except(menus.Select(x => x.Code));
            if (exceptCodes.Any())
            {
                var partMenus = await _menuRepository.GetListAsync(x => exceptCodes.Contains(x.Code));
                menus.AddRange(partMenus);
                menus = menus.OrderBy(x => x.Sort).ToList();
            }
            var menuTree = BuildTreeMenus(menus);
            menuRouterCacheItems = BuildRouter(menuTree);
            return menuRouterCacheItems;
        }

        /// <summary>
        /// 构建前端菜单树
        /// </summary>
        /// <param name="menus"></param>
        /// <returns></returns>
        private static List<Menu> BuildTreeMenus(List<Menu> menus)
        {
            var result = new List<Menu>();
            var queue = new Queue<Menu>();
            var rootMenu = Menu.BuildRoot();
            queue.Enqueue(rootMenu);
            while (queue.Count > 0)
            {
                var parentMenu = queue.Dequeue();
                var childMenus = menus.Where(x => x.ParentCode == parentMenu.Code).ToList();
                parentMenu.SetSubMenu(childMenus);
                foreach (var childMenu in childMenus)
                {
                    queue.Enqueue(childMenu);
                }
            }
            result = rootMenu.SubMenu;
            return result;
        }

        /// <summary>
        /// 构建路由
        /// </summary>
        /// <param name="menus"></param>
        /// <returns></returns>
        private static List<MenuRouterCacheItem> BuildRouter(List<Menu> menus)
        {
            var result = new List<MenuRouterCacheItem>();
            foreach (var menu in menus)
            {
                var menuRouterCacheItem = new MenuRouterCacheItem
                {
                    Name = menu.Name,
                    Hidden = false,
                    Path = menu.UrlAddress,
                    Component = !string.IsNullOrEmpty(menu.ComponentAddress) && menu.Type == MenuConsts.MenuTypeName ? menu.ComponentAddress : "Layout"
                };
                var meta = new Meta(menu.Name, menu.Icon);
                menuRouterCacheItem.Meta = meta;
                var subMenu = menu.SubMenu;
                if (subMenu != null && menu.Type == MenuConsts.CatalogTypeName)
                {
                    menuRouterCacheItem.AlwaysShow = true;
                    menuRouterCacheItem.Redirect = "noRedirect";
                    menuRouterCacheItem.Children = BuildRouter(subMenu);
                }
                result.Add(menuRouterCacheItem);
            }
            return result;
        }
    }
}

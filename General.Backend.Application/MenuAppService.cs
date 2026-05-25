using General.Backend.Application.Contracts.Dtos.Menus;

namespace General.Backend.Application
{
    /// <summary>
    /// 菜单应用服务
    /// </summary>
    public class MenuAppService : ApplicationService, IMenuAppService
    {
        private readonly MenuService _menuService;
        private readonly RoleService _roleService;
        private readonly IRoleRepository _roleRepository;
        private readonly IMenuRepository _menuRepository;

        public MenuAppService(
            MenuService menuService,
            RoleService roleService,
            IRoleRepository roleRepository,
            IMenuRepository menuRepository)
        {
            _menuService = menuService;
            _roleService = roleService;
            _roleRepository = roleRepository;
            _menuRepository = menuRepository;
        }

        /// <summary>
        /// 根据角色编码获取权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PermissionDto> GetPermissionAsync(Guid id)
        {
            var permissionDto = new PermissionDto();
            var role = await _roleRepository.FindAsync(id);
            if (role == null)
            {
                throw new UserFriendlyException("角色不存在");
            }
            var menuCodes = role.RoleMenus.Select(x => x.MenuCode).ToList();
            var allMenus = await _menuRepository.GetListAsync();
            var menus = allMenus.Where(x => menuCodes.Contains(x.Code));
            permissionDto.CheckedMenu = menus.Where(x => x.Type == "C" || x.Type == "M").Select(y => y.Code).ToList();
            permissionDto.CheckedModule = menus.Where(x => x.Type == "F").Select(y => y.Code).ToList();
            var menuDtos = ObjectMapper.Map<List<Menu>, List<MenuDto>>(allMenus);
            menuDtos = menuDtos.OrderBy(x => x.Sort).ToList();
            permissionDto.MenuModule = [];
            menuDtos.ForEach(item =>
            {
                permissionDto.MenuModule.Add(new MenuModuleDto
                {
                    Code = item.Code,
                    ParentCode = item.ParentCode,
                    Name = item.Name,
                    Type = item.Type
                });
            });
            return permissionDto;
        }

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateAsync(PermissionUpdateDto input)
        {
            var role = await _roleRepository.FindAsync(input.RoleId);
            if (role == null)
            {
                throw new UserFriendlyException("角色不存在");
            }
            _roleService.BindMenus(role, input.MenuModules);
            await _roleRepository.UpdateAsync(role);
        }
    }
}

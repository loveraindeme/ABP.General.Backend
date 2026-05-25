namespace General.Backend.Domain.Services
{
    public class RoleService : DomainService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Role> AddAsync(
            string code,
            string name,
            string? remark)
        {
            var sameRoles = await _roleRepository.GetSameRoleAsync(new SameRoleSpecification(name, code));
            if (sameRoles.Count > 0)
            {
                if (sameRoles.Any(role => role.Code == code))
                {
                    throw new UserFriendlyException("已存在相同编码的角色");
                }
                if (sameRoles.Any(role => role.Name == name))
                {
                    throw new UserFriendlyException("已存在相同名称的角色");
                }
            }
            var role = new Role(
                GuidGenerator.Create(),
                code,
                name,
                remark);
            return role;
        }

        public async Task<Role> ModifyAsync(
            Guid id,
            string name,
            string? remark)
        {
            var role = await _roleRepository.FindAsync(id);
            if (role == null)
            {
                throw new UserFriendlyException("角色不存在");
            }
            var sameRoles = await _roleRepository.GetSameRoleAsync(new SameRoleSpecification(name));
            if (sameRoles.Any(item => item.Name == name && item.Id != id))
            {
                throw new UserFriendlyException("已存在相同名称的角色");
            }
            role.SetName(name);
            role.Remark = remark;
            return role;
        }

        public void BindMenus(Role role, List<string> menuCodes)
        {
            var roleMenus = new List<RoleMenu>();
            menuCodes.ForEach(menuCode =>
            {
                var roleMenu = new RoleMenu(
                    GuidGenerator.Create(),
                    role.Id,
                    menuCode);
                roleMenus.Add(roleMenu);
            });
            role.SetMenus(roleMenus);
        }
    }
}

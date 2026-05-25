using General.Backend.Domain.Services;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace General.Backend.Domain.DataSeed
{
    public class GeneralDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private const string AdminAcount = "CBGc4eArEOTOmagzYMIbeA==";
        private const string AdminPassword = "Z1VZUVfa9TB72rz/QvrHDw==";
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public GeneralDataSeedContributor(
            UserService userService,
            RoleService roleService,
            IUserRepository userRepository,
            IRoleRepository roleRepository) 
        {
            _userService = userService;
            _roleService = roleService;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await _userRepository.HardDeleteAsync(user => user.Account == AdminAcount, true);
            var user = await _userService.AddAsync(
                    AdminAcount,
                    AdminPassword,
                    UserConsts.AdminName);
            await _userRepository.InsertAsync(user);

            await _roleRepository.HardDeleteAsync(role => role.Code == RoleConsts.AdminRoleCode, true);
            var role = await _roleService.AddAsync(
                RoleConsts.AdminRoleCode,
                RoleConsts.AdminRoleName,
                RoleConsts.AdminRoleName);
            await _roleRepository.InsertAsync(role);
        }
    }
}

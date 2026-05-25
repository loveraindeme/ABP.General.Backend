using General.Backend.Application.Contracts.Dtos.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Users;

namespace General.Backend.Application
{
    /// <summary>
    /// 用户应用服务
    /// </summary>
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly ILogger<UserAppService> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IDistributedCache<UserPermissionCacheItem, Guid> _permissionCache;
        private readonly UserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRepository<UserRole> _userRoleRepository;

        public UserAppService(ILogger<UserAppService> logger,
            ICurrentUser currentUser,
            IDistributedCache<UserPermissionCacheItem, Guid> permissionCache,
            UserService userService,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IRepository<UserRole> userRoleRepository)
        {
            _logger = logger;
            _currentUser = currentUser;
            _permissionCache = permissionCache;
            _userService = userService;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        /// <summary>
        /// 获取用户分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<UserDto>> GetListAsync(UserQueryDto input)
        {
            string fieldToSort = _userRepository.GenerateSortExpression(input.SortField, input.SortType, typeof(User));

            var query = await _userRepository.GetQueryableAsync();
            var list = await AsyncExecuter.ToListAsync(query);
            list = list
                .WhereIf(!string.IsNullOrEmpty(input.Account) && !string.IsNullOrWhiteSpace(input.Account),
                    user => SecurityHelper.AESDecrypt(user.Account).Contains(input.Account!))
                .WhereIf(!string.IsNullOrEmpty(input.Name) && !string.IsNullOrWhiteSpace(input.Name),
                    user => user.Name.Contains(input.Name!))
                .OrderBy(user => user.CreationTime)
                .ToList();
            var totalCount = list.Count;
            list = list.Skip((input.PageIndex - 1) * input.PageSize).Take(input.PageSize).ToList();

            var userIds = list.Select(user => user.Id);
            var userRoles = await _userRoleRepository.GetListAsync(rel => userIds.Contains(rel.UserId));
            var roleIds = userRoles.Select(rel => rel.RoleId).Distinct().ToList();
            var roles = await _roleRepository.GetListAsync(role => roleIds.Contains(role.Id));
            var listDto = list.Select(user =>
            {
                var userDto = ObjectMapper.Map<User, UserDto>(user);
                userDto.RoleIds = userRoles.Where(rel => rel.UserId == user.Id).Select(rel => rel.RoleId).ToList();
                userDto.RoleNames = roles.Where(x => userDto.RoleIds.Contains(x.Id)).Select(y => y.Name).ToList();
                return userDto;
            }).ToList();

            return new PagedResultDto<UserDto>(totalCount, listDto);
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UserDto> CreateAsync(UserCreateDto input)
        {
            var user = await _userService.AddAsync(
                input.Account,
                input.Password,
                input.Name,
                input.Contact,
                input.Address);
            var roles = await _roleRepository.GetListAsync(role => input.RoleIds.Contains(role.Id));
            var notExistRoles = input.RoleIds.Except(roles.Select(x => x.Id));
            if (notExistRoles.Any())
            {
                throw new UserFriendlyException("角色不存在");
            }
            _userService.BindRoles(user, input.RoleIds);
            user = await _userRepository.InsertAsync(user);
            var userDto = ObjectMapper.Map<User, UserDto>(user);
            return userDto;
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UserDto> UpdateAsync(UserUpdateDto input)
        {
            var user = await _userService.ModifyAsync(
                input.Id,
                input.Password,
                input.Name,
                input.Contact,
                input.Address);
            var roles = await _roleRepository.GetListAsync(role => input.RoleIds.Contains(role.Id));
            var notExistRoles = input.RoleIds.Except(roles.Select(x => x.Id));
            if (notExistRoles.Any())
            {
                throw new UserFriendlyException("角色不存在");
            }
            _userService.BindRoles(user, input.RoleIds);
            user = await _userRepository.UpdateAsync(user);
            var userDto = ObjectMapper.Map<User, UserDto>(user);
            return userDto;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserDto> GetAsync(Guid id)
        {
            var user = await _userRepository.FindAsync(id);
            if (user == null)
            {
                throw new UserFriendlyException("用户不存在");
            }
            var userDto = ObjectMapper.Map<User, UserDto>(user);
            userDto.RoleIds = user.UserRoles.Select(x => x.RoleId).ToList();
            var roles = await _roleRepository.GetListAsync(role => userDto.RoleIds.Contains(role.Id));
            userDto.RoleNames = roles.Select(role => role.Name).ToList();
            return userDto;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task DeleteAsync([FromBody] List<Guid> ids)
        {
            var users = await _userRepository.GetListAsync(user => ids.Contains(user.Id));
            foreach (var user in users)
            {
                var account = SecurityHelper.AESDecrypt(user.Account);
                if (account == UserConsts.AdminAccount)
                {
                    throw new UserFriendlyException("不可操作管理员用户");
                }
                if (_currentUser.Id.HasValue)
                {
                    if (ids.Contains(_currentUser.Id.Value))
                    {
                        throw new UserFriendlyException("不可删除当前用户");
                    }
                }
            }
            await _userRepository.DeleteAsync(user => ids.Contains(user.Id));
            await _permissionCache.RemoveManyAsync(ids);
        }

        /// <summary>
        /// 解冻用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserDto> UnfreezeAsync(Guid id)
        {
            var user = await _userService.UnfreezeAsync(id);
            user = await _userRepository.UpdateAsync(user);
            var userDto = ObjectMapper.Map<User, UserDto>(user);
            return userDto;
        }
    }
}

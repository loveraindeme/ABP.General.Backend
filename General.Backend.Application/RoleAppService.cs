using General.Backend.Application.Contracts.Dtos.Roles;
using Microsoft.AspNetCore.Mvc;

namespace General.Backend.Application
{
    /// <summary>
    /// 角色应用服务
    /// </summary>
    public class RoleAppService : ApplicationService, IRoleAppService
    {
        private readonly RoleService _roleService;
        private readonly IRoleRepository _roleRepository;
        private readonly IRepository<UserRole> _userRoleRepository;

        public RoleAppService(
            RoleService roleService,
            IRoleRepository roleRepository,
            IRepository<UserRole> userRoleRepository
)
        {
            _roleService = roleService;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        /// <summary>
        /// 获取角色分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<RoleDto>> GetListAsync(RoleQueryDto input)
        {
            string fieldToSort = _roleRepository.GenerateSortExpression(input.SortField, input.SortType, typeof(Role));

            var query = await _roleRepository.GetQueryableAsync();
            query = query
                .WhereIf(!string.IsNullOrEmpty(input.Name) && !string.IsNullOrWhiteSpace(input.Name),
                role => role.Name.Contains(input.Name!))
                .OrderByIf<Role, IQueryable<Role>>(!string.IsNullOrEmpty(fieldToSort),
                fieldToSort);
            var totalCount = query.Count();
            query = query.Skip((input.PageIndex - 1) * input.PageSize).Take(input.PageSize);

            var list = await AsyncExecuter.ToListAsync(query);
            var listDto = list.Select(role =>
            {
                var roleDto = ObjectMapper.Map<Role, RoleDto>(role);
                return roleDto;
            }).ToList();

            return new PagedResultDto<RoleDto>(totalCount, listDto);
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RoleDto> CreateAsync(RoleCreateDto input)
        {
            var role = await _roleService.AddAsync(
                input.Code,
                input.Name,
                input.Remark);
            role = await _roleRepository.InsertAsync(role);
            var roleDto = ObjectMapper.Map<Role, RoleDto>(role);
            return roleDto;
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RoleDto> UpdateAsync(RoleUpdateDto input)
        {
            var role = await _roleService.ModifyAsync(
                input.Id,
                input.Name,
                input.Remark);
            role = await _roleRepository.UpdateAsync(role);
            var roleDto = ObjectMapper.Map<Role, RoleDto>(role);
            return roleDto;
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RoleDto> GetAsync(Guid id)
        {
            var role = await _roleRepository.FindAsync(id);
            if (role == null)
            {
                throw new UserFriendlyException("角色不存在");
            }
            var roleDto = ObjectMapper.Map<Role, RoleDto>(role);
            return roleDto;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task DeleteAsync([FromBody] List<Guid> ids)
        {
            var userRoles = await _userRoleRepository.GetListAsync(rel => ids.Contains(rel.RoleId));
            if (userRoles.Count > 0)
            {
                throw new UserFriendlyException("角色已被使用，不可删除");
            }
            await _roleRepository.DeleteAsync(role => ids.Contains(role.Id));
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleMapDto>> GetAllRoleAsync()
        {
            var roles = await _roleRepository.GetListAsync();
            return ObjectMapper.Map<List<Role>, List<RoleMapDto>>(roles);
        }
    }
}

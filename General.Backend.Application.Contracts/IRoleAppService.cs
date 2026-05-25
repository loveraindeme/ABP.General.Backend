using General.Backend.Application.Contracts.Dtos.Roles;

namespace General.Backend.Application.Contracts
{
    /// <summary>
    /// 角色应用服务
    /// </summary>
    public interface IRoleAppService : IApplicationService
    {
        /// <summary>
        /// 获取角色分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<RoleDto>> GetListAsync(RoleQueryDto input);

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<RoleDto> CreateAsync(RoleCreateDto input);

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<RoleDto> UpdateAsync(RoleUpdateDto input);

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<RoleDto> GetAsync(Guid id);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task DeleteAsync(List<Guid> ids);

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public Task<List<RoleMapDto>> GetAllRoleAsync();
    }
}

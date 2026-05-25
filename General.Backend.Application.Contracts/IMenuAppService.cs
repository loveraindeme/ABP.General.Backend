using General.Backend.Application.Contracts.Dtos.Menus;

namespace General.Backend.Application.Contracts
{
    /// <summary>
    /// 菜单应用服务
    /// </summary>
    public interface IMenuAppService : IApplicationService
    {
        /// <summary>
        /// 根据角色编码获取权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<PermissionDto> GetPermissionAsync(Guid id);

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task UpdateAsync(PermissionUpdateDto input);
    }
}

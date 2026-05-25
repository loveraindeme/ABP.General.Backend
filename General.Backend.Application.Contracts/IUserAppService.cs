using General.Backend.Application.Contracts.Dtos.Users;

namespace General.Backend.Application.Contracts
{
    /// <summary>
    /// 用户应用服务
    /// </summary>
    public interface IUserAppService : IApplicationService
    {
        /// <summary>
        /// 获取用户分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<UserDto>> GetListAsync(UserQueryDto input);

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<UserDto> CreateAsync(UserCreateDto input);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<UserDto> UpdateAsync(UserUpdateDto input);

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<UserDto> GetAsync(Guid id);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task DeleteAsync(List<Guid> ids);

        /// <summary>
        /// 解冻用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<UserDto> UnfreezeAsync(Guid id);
    }
}

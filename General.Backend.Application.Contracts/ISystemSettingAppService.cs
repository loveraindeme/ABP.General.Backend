using General.Backend.Application.Contracts.Dtos.SystemSettings;

namespace General.Backend.Application.Contracts
{
    /// <summary>
    /// 系统设置应用服务
    /// </summary>
    public interface ISystemSettingAppService : IApplicationService
    {
        /// <summary>
        /// 获取系统设置
        /// </summary>
        /// <returns></returns>
        public Task<List<SystemSettingDto>> GetListAsync();

        /// <summary>
        /// 更新系统设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task UpdateAsync(SystemSettingsUpdateDto input);
    }
}

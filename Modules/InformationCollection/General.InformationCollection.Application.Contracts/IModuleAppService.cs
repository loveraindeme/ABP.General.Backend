using General.InformationCollection.Application.Contracts.Dtos.Modules;

namespace General.InformationCollection.Application.Contracts
{
    /// <summary>
    /// 模块信息应用服务
    /// </summary>
    public interface IModuleAppService : IApplicationService
    {
        /// <summary>
        /// 获取所有模块信息
        /// </summary>
        /// <returns></returns>
        public Task<List<ModuleDto>> GetAllModuleAsync();

        /// <summary>
        /// 根据模块名称获取模块信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<ModuleDto> GetByNameAsync(string name);
    }
}

using General.InformationCollection.Application.Contracts.Dtos.Modules;
using General.InformationCollection.Domain.Entities;

namespace General.InformationCollection.Application
{
    /// <summary>
    /// 模块信息应用服务
    /// </summary>
    public class ModuleAppService : ApplicationService, IModuleAppService
    {
        private readonly IModuleRepository _moduleRepository;

        public ModuleAppService(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        /// <summary>
        /// 获取所有模块信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<ModuleDto>> GetAllModuleAsync()
        {
            var query = await _moduleRepository.GetQueryableAsync();
            query = query.OrderBy(module => module.Id);
            var modules = await AsyncExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<Module>, List<ModuleDto>>(modules);
        }

        /// <summary>
        /// 根据模块名称获取模块信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ModuleDto> GetByNameAsync(string name)
        {
            var module = await _moduleRepository.FindAsync(x => x.Name == name);
            if (module == null)
            {
                throw new UserFriendlyException("模块不存在");
            }

            return ObjectMapper.Map<Module, ModuleDto>(module);
        }
    }
}

using General.Backend.Application.Contracts.Dtos.SystemSettings;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace General.Backend.Application
{
    /// <summary>
    /// 系统设置应用服务
    /// </summary>
    public class SystemSettingAppService : ApplicationService, ISystemSettingAppService
    {
        private readonly ISettingManager _settingManager;
        private readonly SystemSettingService _systemSettingService;

        public SystemSettingAppService(
            ISettingManager settingManager,
            SystemSettingService systemSettingService)
        {
            _settingManager = settingManager;
            _systemSettingService = systemSettingService;
        }

        /// <summary>
        /// 获取系统设置
        /// </summary>
        /// <returns></returns>
        public async Task<List<SystemSettingDto>> GetListAsync()
        {
            var definitionMap = await _systemSettingService.GetSystemSettingDefinitionMapAsync();
            var systemSettingDtos = new List<SystemSettingDto>();
            foreach (var definition in definitionMap.Values)
            {
                systemSettingDtos.Add(await GetSettingDtoAsync(definition.Name, definition));
            }
            systemSettingDtos = systemSettingDtos
                .OrderBy(setting => setting.Group)
                .ThenBy(setting => setting.Sort)
                .ToList();
            return systemSettingDtos;
        }

        /// <summary>
        /// 更新系统设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateAsync(SystemSettingsUpdateDto input)
        {
            if (input.Settings.Count == 0)
            {
                throw new UserFriendlyException("设置项不得为空");
            }

            var definitionMap = await _systemSettingService.GetSystemSettingDefinitionMapAsync();
            foreach (var setting in input.Settings)
            {
                var definition = _systemSettingService.GetSupportedSettingDefinition(setting.Name, definitionMap);
                _systemSettingService.ValidateValueByMeta(definition, setting.Value ?? string.Empty);
                
                await _settingManager.SetGlobalAsync(setting.Name, setting.Value ?? string.Empty);
            }
        }

        private async Task<SystemSettingDto> GetSettingDtoAsync(string settingName, SettingDefinition definition)
        {
            var value = await _settingManager.GetOrNullGlobalAsync(settingName);
            var metaInfo = _systemSettingService.GetMetaInfo(definition);
            return new SystemSettingDto
            {
                Name = settingName,
                DisplayName = metaInfo.DisplayName?.Localize(StringLocalizerFactory) ?? settingName,
                Value = value,
                ValueType = metaInfo.ValueType,
                Required = metaInfo.Required,
                Group = metaInfo.Group,
                Sort = metaInfo.Sort,
                IsSecret = metaInfo.IsSecret,
                Regex = metaInfo.Regex,
                Min = metaInfo.Min,
                Max = metaInfo.Max,
                Options = metaInfo.Options
            };
        }
    }
}

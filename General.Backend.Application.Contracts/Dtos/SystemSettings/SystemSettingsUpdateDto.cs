using Volo.Abp.Settings;

namespace General.Backend.Application.Contracts.Dtos.SystemSettings
{
    /// <summary>
    /// 系统设置更新
    /// </summary>
    public class SystemSettingsUpdateDto
    {
        /// <summary>
        /// 设置项列表
        /// </summary>
        public List<SettingValue> Settings { get; set; } = [];
    }
}

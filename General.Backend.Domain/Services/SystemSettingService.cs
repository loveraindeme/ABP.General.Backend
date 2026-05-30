using System.Text.Json;
using System.Text.RegularExpressions;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace General.Backend.Domain.Services
{
    public class SystemSettingService : DomainService
    {
        private readonly ISettingDefinitionManager _settingDefinitionManager;

        public SystemSettingService(ISettingDefinitionManager settingDefinitionManager)
        {
            _settingDefinitionManager = settingDefinitionManager;
        }

        public async Task<Dictionary<string, SettingDefinition>> GetSystemSettingDefinitionMapAsync()
        {
            var definitions = await _settingDefinitionManager.GetAllAsync();
            return definitions
                .Where(definition => definition.Name.StartsWith(SystemSettingConsts.SystemSettingPrefix))
                .ToDictionary(definition => definition.Name, definition => definition);
        }

        public SettingDefinition GetSupportedSettingDefinition(string settingName, IReadOnlyDictionary<string, SettingDefinition> definitionMap)
        {
            if (!definitionMap.TryGetValue(settingName, out var definition))
            {
                throw new UserFriendlyException($"不支持设置项{settingName}");
            }

            return definition;
        }

        public SystemSettingMetaInfo GetMetaInfo(SettingDefinition definition)
        {
            return new SystemSettingMetaInfo
            {
                DisplayName = definition.DisplayName,
                ValueType = GetMetaString(definition, SystemSettingMetaConsts.ValueType),
                Required = GetMetaBool(definition, SystemSettingMetaConsts.Required),
                Group = GetMetaString(definition, SystemSettingMetaConsts.Group),
                Sort = GetMetaInt(definition, SystemSettingMetaConsts.Sort),
                IsSecret = GetMetaBool(definition, SystemSettingMetaConsts.IsSecret),
                Regex = GetMetaString(definition, SystemSettingMetaConsts.Regex),
                Min = GetMetaString(definition, SystemSettingMetaConsts.Min),
                Max = GetMetaString(definition, SystemSettingMetaConsts.Max),
                Options = GetMetaString(definition, SystemSettingMetaConsts.Options)
            };
        }

        public void ValidateValueByMeta(SettingDefinition definition, string value)
        {
            var required = GetMetaBool(definition, SystemSettingMetaConsts.Required);
            if (required && string.IsNullOrWhiteSpace(value))
            {
                throw new UserFriendlyException($"设置项{definition.Name}为必填");
            }

            var valueType = GetMetaString(definition, SystemSettingMetaConsts.ValueType);
            ValidateType(definition.Name, valueType, value);
            ValidateRegex(definition, value);
            ValidateRange(definition, value);
            ValidateOptions(definition, value);
        }

        private static void ValidateType(string settingName, string valueType, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var valid = valueType switch
            {
                SystemSettingValueTypeConsts.ValueTypeString => true,
                SystemSettingValueTypeConsts.ValueTypeInt => int.TryParse(value, out _),
                SystemSettingValueTypeConsts.ValueTypeBool => bool.TryParse(value, out _),
                SystemSettingValueTypeConsts.ValueTypeDecimal => decimal.TryParse(value, out _),
                SystemSettingValueTypeConsts.ValueTypeDateTime => DateTime.TryParse(value, out _),
                SystemSettingValueTypeConsts.ValueTypeJson => TryParseJson(value),
                _ => true
            };

            if (!valid)
            {
                throw new UserFriendlyException($"设置项{settingName}的值类型无效，应为{valueType}");
            }
        }

        private static void ValidateRegex(SettingDefinition definition, string value)
        {
            var regex = GetMetaString(definition, SystemSettingMetaConsts.Regex);
            if (string.IsNullOrWhiteSpace(regex) || string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            if (!Regex.IsMatch(value, regex))
            {
                throw new UserFriendlyException($"设置项{definition.Name}不符合正则规则");
            }
        }

        private static void ValidateRange(SettingDefinition definition, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var min = GetMetaString(definition, SystemSettingMetaConsts.Min);
            var max = GetMetaString(definition, SystemSettingMetaConsts.Max);
            if (string.IsNullOrWhiteSpace(min) && string.IsNullOrWhiteSpace(max))
            {
                return;
            }

            if (!decimal.TryParse(value, out var currentValue))
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(min) && decimal.TryParse(min, out var minValue) && currentValue < minValue)
            {
                throw new UserFriendlyException($"设置项{definition.Name}小于最小值{minValue}");
            }

            if (!string.IsNullOrWhiteSpace(max) && decimal.TryParse(max, out var maxValue) && currentValue > maxValue)
            {
                throw new UserFriendlyException($"设置项{definition.Name}大于最大值{maxValue}");
            }
        }

        private static void ValidateOptions(SettingDefinition definition, string value)
        {
            var options = GetMetaString(definition, SystemSettingMetaConsts.Options);
            if (string.IsNullOrWhiteSpace(options) || string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var optionValues = ParseOptions(options);
            if (optionValues.Count > 0 && !optionValues.Any(v => v.Value == value))
            {
                throw new UserFriendlyException($"设置项{definition.Name}的{value}不在允许选项中");
            }
        }

        private static bool TryParseJson(string value)
        {
            try
            {
                using var _ = JsonDocument.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static List<NameValue> ParseOptions(string options)
        {
            try
            {
                var nameValues = JsonSerializer.Deserialize<List<NameValue>>(options);
                return nameValues ?? [];
            }
            catch
            {
                return [];
            }
        }

        private static string GetMetaString(SettingDefinition definition, string key)
        {
            if (!definition.Properties.TryGetValue(key, out var value) || value == null)
            {
                return string.Empty;
            }

            return value.ToString() ?? string.Empty;
        }

        private static int GetMetaInt(SettingDefinition definition, string key)
        {
            if (!definition.Properties.TryGetValue(key, out var value) || value == null)
            {
                return 0;
            }
            return value switch
            {
                int intValue => intValue,
                _ => int.TryParse(value.ToString(), out var parsed) ? parsed : 1
            };
        }

        private static bool GetMetaBool(SettingDefinition definition, string key)
        {
            if (!definition.Properties.TryGetValue(key, out var value) || value == null)
            {
                return false;
            }

            return value switch
            {
                bool boolValue => boolValue,
                _ => bool.TryParse(value.ToString(), out var parsed) && parsed
            };
        }
    }

    public class SystemSettingMetaInfo
    {
        public ILocalizableString? DisplayName { get; set; }

        public string ValueType { get; set; } = string.Empty;

        public bool Required { get; set; }

        public string Group { get; set; } = string.Empty;

        public int Sort { get; set; }

        public bool IsSecret { get; set; }

        public string Regex { get; set; } = string.Empty;

        public string Min { get; set; } = string.Empty;

        public string Max { get; set; } = string.Empty;

        public string Options { get; set; } = string.Empty;
    }
}

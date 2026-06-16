namespace General.Backend.Application.Contracts.Dtos.SystemSettings
{
    /// <summary>
    /// 系统设置项
    /// </summary>
    public class SystemSettingDto : EntityDto<Guid>
    {
        /// <summary>
        /// 设置名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 展示名称
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// 设置值
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// 值类型
        /// </summary>
        public string ValueType { get; set; } = string.Empty;

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        public string? Group { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否敏感字段
        /// </summary>
        public bool IsSecret { get; set; }

        /// <summary>
        /// 正则表达式
        /// </summary>
        public string? Regex { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public string? Min { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public string? Max { get; set; }

        /// <summary>
        /// 选项数组
        /// </summary>
        public string? Options { get; set; }
    }
}

namespace General.InformationCollection.Application.Contracts.Dtos.Modules
{
    /// <summary>
    /// 模块信息
    /// </summary>
    public class ModuleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 编译版本
        /// </summary>
        public string CompiledVersion { get; set; } = string.Empty;

        /// <summary>
        /// 编译时间
        /// </summary>
        public string CompiledTime { get; set; } = string.Empty;

        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}

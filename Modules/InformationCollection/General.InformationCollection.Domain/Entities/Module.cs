using General.InformationCollection.Domain.Shared.Consts;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace General.InformationCollection.Domain.Entities
{
    /// <summary>
    /// 模块
    /// </summary>
    public class Module : BasicAggregateRoot<Guid>, IHasExtraProperties, IHasCreationTime
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public virtual string Name { get; private set; } = string.Empty;

        /// <summary>
        /// 编译版本
        /// </summary>
        public virtual string CompiledVersion {  get; private set; } = string.Empty;

        /// <summary>
        /// 编译时间
        /// </summary>
        public virtual string CompiledTime { get; private set; } = string.Empty;

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string? Description { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public virtual ExtraPropertyDictionary ExtraProperties { get; private set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

        protected Module()
        {

        }

        public Module(
            [NotNull] string name,
            [NotNull] string compiledVersion,
            [NotNull] string compiledTime,
            [CanBeNull] string? description = null)
        {
            SetName(name);
            SetCompiledVersion(compiledVersion);
            SetCompiledTime(compiledTime);
            SetDescription(description);
            ExtraProperties = [];
        }

        private void SetName([NotNull] string name)
        {
            Name = Check.Length(name, nameof(name), ModuleConsts.MaxNameLength, ModuleConsts.MinNameLength)!;
        }

        public virtual void SetCompiledVersion([NotNull] string compiledVersion)
        {
            CompiledVersion = Check.Length(compiledVersion, nameof(compiledVersion), ModuleConsts.MaxCompiledVersionLength, ModuleConsts.MinCompiledVersionLength)!;
        }

        public virtual void SetCompiledTime([NotNull] string compiledTime)
        {
            CompiledTime = Check.Length(compiledTime, nameof(compiledTime), ModuleConsts.MaxCompiledTimeLength, ModuleConsts.MinCompiledTimeLength)!;
        }

        public virtual void SetDescription([CanBeNull] string? description)
        {
            Description = Check.Length(description, nameof(description), ModuleConsts.MaxDescriptionLength);
        }

        public virtual Module WithProperty(string key, object value)
        {
            ExtraProperties[key] = value;
            return this;
        }
    }
}

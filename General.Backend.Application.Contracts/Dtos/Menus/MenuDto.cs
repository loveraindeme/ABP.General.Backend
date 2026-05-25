namespace General.Backend.Application.Contracts.Dtos.Menus
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuDto : EntityDto<Guid>, IHasCreationTime
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 父编码
        /// </summary>
        public string ParentCode { get; set; } = string.Empty;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; } = string.Empty;

        /// <summary>
        /// 路由地址
        /// </summary>
        public string UrlAddress { get; set; } = string.Empty;

        /// <summary>
        /// 组件地址
        /// </summary>
        public string ComponentAddress { get; set; } = string.Empty;

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; }
    }
}

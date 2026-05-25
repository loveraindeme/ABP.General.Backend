namespace General.Backend.Domain.Entities
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class Menu : BasicAggregateRoot<Guid>, IHasCreationTime
    {
        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; private set; } = string.Empty;

        /// <summary>
        /// 父编码
        /// </summary>
        public virtual string ParentCode { get; private set; } = string.Empty;

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; private set; } = string.Empty;

        /// <summary>
        /// 类型
        /// </summary>
        public virtual string Type { get; private set; } = string.Empty;

        /// <summary>
        /// 层级
        /// </summary>
        public virtual int Level { get; private set; }

        /// <summary>
        /// 图标
        /// </summary>
        public virtual string? Icon { get; private set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        public virtual string? UrlAddress { get; private set; }

        /// <summary>
        /// 组件地址
        /// </summary>
        public virtual string? ComponentAddress { get; private set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int Sort { get; private set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; private set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public List<Menu> SubMenu { get; private set; } = [];

        protected Menu()
        {

        }

        public Menu(
            [NotNull] string code,
            [NotNull] string parentCode,
            [NotNull] string name,
            [NotNull] string type,
            int level,
            int sort,
            [CanBeNull] string? icon = null,
            [CanBeNull] string? urlAddress = null,
            [CanBeNull] string? componentAddress = null)
        {
            Code = Check.Length(code, nameof(code), MenuConsts.MaxCodeLength, MenuConsts.MinCodeLength)!;
            ParentCode = Check.Length(parentCode, nameof(parentCode), MenuConsts.MaxParentCodeLength, MenuConsts.MinParentCodeLength)!;
            Name = Check.Length(name, nameof(name), MenuConsts.MaxNameLength, MenuConsts.MinNameLength)!;
            Type = Check.Length(type, nameof(type), MenuConsts.MaxTypeLength, MenuConsts.MinTypeLength)!;
            Level = level;
            Sort = sort;
            Icon = Check.Length(icon, nameof(icon), MenuConsts.MaxIconLength);
            UrlAddress = Check.Length(urlAddress, nameof(urlAddress), MenuConsts.MaxUrlAddressLength);
            ComponentAddress = Check.Length(componentAddress, nameof(componentAddress), MenuConsts.MaxComponentAddressLength);
        }

        private Menu(
            [NotNull] string code)
        {
            Code = Check.Length(code, nameof(code), MenuConsts.MaxCodeLength, MenuConsts.MinCodeLength)!;
        }

        internal static Menu BuildRoot()
        {
            return new Menu(
                MenuConsts.MenuRootCode);
        }

        internal static Menu BuildCatalog(
            [NotNull] string code,
            [NotNull] string name,
            [NotNull] string icon,
            [NotNull] string urlAddress,
            int sort)
        {
            return new Menu(
                code,
                MenuConsts.MenuRootCode,
                name,
                MenuConsts.CatalogTypeName,
                1,
                sort,
                icon,
                urlAddress);
        }

        internal static Menu BuildMenu(
            [NotNull] string code,
            [NotNull] string parentCode,
            [NotNull] string name,
            [NotNull] string icon,
            [NotNull] string urlAddress,
            [NotNull] string componentAddress,
            int sort)
        {
            return new Menu(
                code,
                parentCode,
                name,
                MenuConsts.MenuTypeName,
                2,
                sort,
                icon,
                urlAddress,
                componentAddress);
        }

        internal static Menu BuildFunc(
            [NotNull] string code,
            [NotNull] string parentCode,
            [NotNull] string name,
            int sort)
        {
            return new Menu(
                code,
                parentCode,
                name,
                MenuConsts.FuncTypeName,
                3,
                sort);
        }

        public void SetSubMenu(
            [NotNull] List<Menu> subMenus)
        {
            Check.NotNull(subMenus, nameof(subMenus));
            foreach (var parentCode in subMenus.Select(menu => menu.ParentCode))
            {
                Check.Equals(Code, parentCode);
            }
            SubMenu = subMenus;
        }
    }
}

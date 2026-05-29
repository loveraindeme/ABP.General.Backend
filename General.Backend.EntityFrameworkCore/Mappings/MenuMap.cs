namespace General.Backend.EntityFrameworkCore.Mappings
{
    /// <summary>
    /// 菜单实体映射配置
    /// </summary>
    public class MenuMap : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable(MenuConsts.MenuTableName, table =>
            {
                table.HasComment(MenuConsts.MenuTableComment);
            });
            builder.ConfigureByConvention();

            builder.Property(menu => menu.Id)
                .HasComment("主键");

            builder.Property(menu => menu.Code)
                .HasMaxLength(MenuConsts.MaxCodeLength)
                .IsRequired()
                .HasComment("编码");

            builder.Property(menu => menu.ParentCode)
                .HasMaxLength(MenuConsts.MaxParentCodeLength)
                .IsRequired()
                .HasComment("父编码");

            builder.Property(menu => menu.Name)
                .HasMaxLength(MenuConsts.MaxNameLength)
                .IsRequired()
                .HasComment("名称");

            builder.Property(menu => menu.Type)
                .HasMaxLength(MenuConsts.MaxTypeLength)
                .IsRequired()
                .HasComment("类型");

            builder.Property(menu => menu.Level)
                .IsRequired()
                .HasComment("层级");

            builder.Property(menu => menu.Icon)
                .HasMaxLength(MenuConsts.MaxIconLength)
                .HasComment("图标");

            builder.Property(menu => menu.UrlAddress)
                .HasMaxLength(MenuConsts.MaxUrlAddressLength)
                .HasComment("路由地址");

            builder.Property(menu => menu.ComponentAddress)
                .HasMaxLength(MenuConsts.MaxComponentAddressLength)
                .HasComment("组件地址");

            builder.Property(menu => menu.Sort)
                .IsRequired()
                .HasComment("排序");

            builder.Property(menu => menu.CreationTime)
                .HasComment("创建时间");

            builder.Ignore(menu => menu.SubMenu);
        }
    }
}

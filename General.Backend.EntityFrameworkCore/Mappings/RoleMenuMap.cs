namespace General.Backend.EntityFrameworkCore.Mappings
{
    /// <summary>
    /// 角色菜单实体映射配置
    /// </summary>
    public class RoleMenuMap : IEntityTypeConfiguration<RoleMenu>
    {
        public void Configure(EntityTypeBuilder<RoleMenu> builder)
        {
            builder.ToTable(RoleConsts.RoleMenuTableName, table =>
            {
                table.HasComment(RoleConsts.RoleMenuTableComment);
            });
            builder.ConfigureByConvention();

            builder.Property(rel => rel.Id)
                .HasComment("主键");

            builder.Property(rel => rel.RoleId)
                .IsRequired()
                .HasComment("角色Id");

            builder.Property(rel => rel.MenuCode)
                .HasMaxLength(MenuConsts.MaxCodeLength)
                .IsRequired()
                .HasComment("菜单编码");

            builder.Property(rel => rel.CreationTime)
                .HasComment("创建时间");
        }
    }
}

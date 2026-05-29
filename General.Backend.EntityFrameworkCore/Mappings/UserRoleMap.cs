namespace General.Backend.EntityFrameworkCore.Mappings
{
    /// <summary>
    /// 用户角色实体映射配置
    /// </summary>
    public class UserRoleMap : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable(UserConsts.UserRoleTableName, table =>
            {
                table.HasComment(UserConsts.UserRoleTableComment);
            });
            builder.ConfigureByConvention();

            builder.Property(rel => rel.Id)
                .HasComment("主键");

            builder.Property(rel => rel.UserId)
                .IsRequired()
                .HasComment("用户Id");

            builder.Property(rel => rel.RoleId)
                .IsRequired()
                .HasComment("角色Id");

            builder.Property(rel => rel.CreationTime)
                .HasComment("创建时间");
        }
    }
}

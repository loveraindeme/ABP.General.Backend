
namespace General.Backend.EntityFrameworkCore.Mappings
{
    /// <summary>
    /// 角色实体映射配置
    /// </summary>
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(RoleConsts.RoleTableName, table =>
            {
                table.HasComment(RoleConsts.RoleTableComment);
            });
            builder.ConfigureByConvention();

            builder.Property(role => role.Id)
                .HasComment("主键");

            builder.Property(role => role.Code)
                .HasMaxLength(RoleConsts.MaxCodeLength)
                .IsRequired()
                .HasComment("编码");

            builder.Property(role => role.Name)
                .HasMaxLength(RoleConsts.MaxNameLength)
                .IsRequired()
                .HasComment("名称");

            builder.Property(role => role.Remark)
                .HasMaxLength(RoleConsts.MaxRemarkLength)
                .HasComment("备注");

            builder.Property(role => role.CreationTime)
                .HasComment("创建时间");

            builder.Property(role => role.CreatorId)
                .HasComment("创建人");

            builder.Property(role => role.LastModificationTime)
                .HasComment("更新时间");

            builder.Property(role => role.LastModifierId)
                .HasComment("更新人");

            builder.Property(role => role.DeletionTime)
                .HasComment("删除时间");

            builder.Property(role => role.DeleterId)
                .HasComment("删除人");

            builder.Property(role => role.IsDeleted)
                .HasComment("是否删除，0：未删除，1：已删除");

            builder.HasMany(role => role.RoleMenus)
                .WithOne()
                .HasForeignKey(rel => rel.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

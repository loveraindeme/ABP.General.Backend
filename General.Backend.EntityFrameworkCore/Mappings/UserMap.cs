namespace General.Backend.EntityFrameworkCore.Mappings
{
    /// <summary>
    /// 用户实体映射配置
    /// </summary>
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(UserConsts.UserTableName, table =>
            {
                table.HasComment(UserConsts.UserTableComment);
            });
            builder.ConfigureByConvention();

            builder.Property(user => user.Id)
                .HasComment("主键");

            builder.Property(user => user.Account)
                .HasMaxLength(UserConsts.MaxAccountLength)
                .IsRequired()
                .HasComment("账号");

            builder.Property(user => user.Password)
                .HasMaxLength(UserConsts.MaxPasswordLength)
                .IsRequired()
                .HasComment("密码");

            builder.Property(user => user.Name)
                .HasMaxLength(UserConsts.MaxNameLength)
                .IsRequired()
                .HasComment("姓名");

            builder.Property(user => user.Contact)
                .HasMaxLength(UserConsts.MaxContactLength)
                .HasComment("联系信息");

            builder.Property(user => user.Address)
                .HasMaxLength(UserConsts.MaxAddressLength)
                .HasComment("地址");

            builder.Property(user => user.IsFrozen)
                .IsRequired()
                .HasComment("是否冻结，1：未冻结，2：已冻结");

            builder.Property(user => user.LoginErrorCount)
                .IsRequired()
                .HasComment("登录错误次数");

            builder.Property(user => user.CreationTime)
                .HasComment("创建时间");

            builder.Property(user => user.CreatorId)
                .HasComment("创建人");

            builder.Property(user => user.LastModificationTime)
                .HasComment("更新时间");

            builder.Property(user => user.LastModifierId)
                .HasComment("更新人");

            builder.Property(user => user.DeletionTime)
                .HasComment("删除时间");

            builder.Property(user => user.DeleterId)
                .HasComment("删除人");

            builder.Property(user => user.IsDeleted)
                .HasComment("是否删除，0：未删除，1：已删除");

            builder.HasMany(user => user.UserRoles)
                .WithOne()
                .HasForeignKey(rel => rel.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

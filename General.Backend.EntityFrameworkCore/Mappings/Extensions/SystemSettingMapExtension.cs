using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace General.Backend.EntityFrameworkCore.Mappings.Extensions
{
    public static class SystemSettingMapExtension
    {
        public static void ConfigureSystemSetting([NotNull] this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.ConfigureSettingManagement();

            builder.Entity<Setting>(builder =>
            {
                builder.ToTable(SystemSettingConsts.SettingTableName, table =>
                {
                    table.HasComment(SystemSettingConsts.SettingTableComment);
                });

                builder.Property(setting => setting.Id)
                    .HasComment("主键");

                builder.Property(setting => setting.Name)
                    .HasComment("设置名称");

                builder.Property(setting => setting.Value)
                    .HasComment("设置值");

                builder.Property(setting => setting.ProviderName)
                    .HasComment("提供者名称");

                builder.Property(setting => setting.ProviderKey)
                    .HasComment("提供者键");
            });

            builder.Entity<SettingDefinitionRecord>(builder =>
            {
                builder.ToTable(SystemSettingConsts.SettingDefinitionTableName, table =>
                {
                    table.HasComment(SystemSettingConsts.SettingDefinitionTableComment);
                });

                builder.Property(settingDefinition => settingDefinition.Id)
                    .HasComment("主键");

                builder.Property(settingDefinition => settingDefinition.Name)
                    .HasComment("设置名称");

                builder.Property(settingDefinition => settingDefinition.DisplayName)
                    .HasComment("显示名称");

                builder.Property(settingDefinition => settingDefinition.Description)
                    .HasComment("描述");

                builder.Property(settingDefinition => settingDefinition.DefaultValue)
                    .HasComment("默认值");

                builder.Property(settingDefinition => settingDefinition.IsVisibleToClients)
                    .HasComment("是否对客户端可见");

                builder.Property(settingDefinition => settingDefinition.Providers)
                    .HasComment("支持提供者列表");

                builder.Property(settingDefinition => settingDefinition.IsInherited)
                    .HasComment("是否允许继承");

                builder.Property(settingDefinition => settingDefinition.IsEncrypted)
                    .HasComment("是否加密存储");
            });
        }
    }
}

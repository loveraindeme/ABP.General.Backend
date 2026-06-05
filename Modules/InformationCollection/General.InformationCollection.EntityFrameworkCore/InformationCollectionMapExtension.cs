using General.InformationCollection.Domain.Entities;
using General.InformationCollection.Domain.Shared.Consts;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace General.InformationCollection.EntityFrameworkCore
{
    public static class InformationCollectionMapExtension
    {
        public static void ConfigureInformationCollection([NotNull] this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<Module>(b =>
            {
                b.ToTable(ModuleConsts.ModuleTableName, table =>
                {
                    table.HasComment(ModuleConsts.ModuleTableComment);
                });

                b.ConfigureByConvention();

                b.Property(module => module.Id)
                    .HasComment("主键");

                b.Property(module => module.Name)
                    .HasMaxLength(ModuleConsts.MaxNameLength)
                    .IsRequired()
                    .HasComment("名称");

                b.Property(module => module.CompiledVersion)
                    .HasMaxLength(ModuleConsts.MaxCompiledVersionLength)
                    .IsRequired()
                    .HasComment("编译版本");

                b.Property(module => module.CompiledTime)
                    .HasMaxLength(ModuleConsts.MaxCompiledTimeLength)
                    .IsRequired()
                    .HasComment("编译时间");

                b.Property(module => module.Description)
                    .HasMaxLength(ModuleConsts.MaxDescriptionLength)
                    .HasComment("描述");

                b.Property(module => module.CreationTime)
                    .HasComment("创建时间");
            });
        }
    }
}

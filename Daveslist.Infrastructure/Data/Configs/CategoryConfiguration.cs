using Daveslist.Domain.Categories.Models;
using Daveslist.Domain.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Daveslist.Infrastructure.Data.Configs;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
               .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
               .HasMaxLength(DomainRulesConstants.Category.MaxNameLength);

        builder.HasIndex(c => c.Name)
               .IsUnique();
    }
}

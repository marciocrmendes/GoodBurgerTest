using GoodBurger.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodBurger.Api.Infrastructure.Persistence.Configurations;

public class ItemCategoryConfiguration : IEntityTypeConfiguration<ItemCategory>
{
    public void Configure(EntityTypeBuilder<ItemCategory> builder)
    {
        builder.HasQueryFilter(b => b.DeletedAt == null);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Active).IsRequired();

        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(propertyExpression: u => u.CreatedBy)
            .IsRequired();

        builder.Property(u => u.UpdatedAt);
        builder.Property(propertyExpression: u => u.UpdatedBy);
        builder.Property(u => u.DeletedAt);
        builder.Property(propertyExpression: u => u.DeletedBy);
    }
}

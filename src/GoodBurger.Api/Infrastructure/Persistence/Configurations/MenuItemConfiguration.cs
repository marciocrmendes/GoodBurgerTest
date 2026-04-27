using GoodBurger.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodBurger.Api.Infrastructure.Persistence.Configurations;

public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.HasQueryFilter(b => b.DeletedAt == null);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Price).HasColumnType("numeric(10,2)").IsRequired();

        builder.HasOne(x => x.ItemCategory)
               .WithMany()
               .HasForeignKey(x => x.ItemCategoryId)
               .OnDelete(DeleteBehavior.Restrict);

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

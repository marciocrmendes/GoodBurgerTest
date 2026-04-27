using GoodBurger.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodBurger.Api.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasQueryFilter(b => b.DeletedAt == null);

        builder.HasKey(x => new { x.OrderId, x.MenuItemId });

        builder.Property(x => x.UnitPrice).HasColumnType("numeric(10,2)").IsRequired();

        builder.HasOne(x => x.Order)
               .WithMany(o => o.Items)
               .HasForeignKey(x => x.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.MenuItem)
               .WithMany()
               .HasForeignKey(x => x.MenuItemId)
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

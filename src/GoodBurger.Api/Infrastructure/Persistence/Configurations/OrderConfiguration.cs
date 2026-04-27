using GoodBurger.Api.Domain.Entities;
using GoodBurger.Api.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodBurger.Api.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasQueryFilter(b => b.DeletedAt == null);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Subtotal).HasColumnType("numeric(10,2)").IsRequired();
        builder.Property(x => x.DiscountPercentage).HasColumnType("numeric(5,2)").IsRequired();
        builder.Property(x => x.DiscountAmount).HasColumnType("numeric(10,2)").IsRequired();
        builder.Property(x => x.Total).HasColumnType("numeric(10,2)").IsRequired();
        builder.Property(x => x.Status).IsRequired().HasDefaultValue(OrderStatus.ToDo);

        builder.HasMany(x => x.Items)
               .WithOne(i => i.Order)
               .HasForeignKey(i => i.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

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

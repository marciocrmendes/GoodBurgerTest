using GoodBurger.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodBurger.Api.Infrastructure.Persistence.Configurations;

public class ComboConfiguration : IEntityTypeConfiguration<Combo>
{
    public void Configure(EntityTypeBuilder<Combo> builder)
    {
        builder.HasQueryFilter(b => b.DeletedAt == null);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(200);
        builder.Property(x => x.DiscountPercentage).HasColumnType("numeric(5,2)").IsRequired();

        builder.HasMany(x => x.Items)
               .WithOne(i => i.Combo)
               .HasForeignKey(i => i.ComboId)
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

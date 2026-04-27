using GoodBurger.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodBurger.Api.Infrastructure.Persistence.Configurations;

public class ComboItemConfiguration : IEntityTypeConfiguration<ComboItem>
{
    public void Configure(EntityTypeBuilder<ComboItem> builder)
    {
        builder.HasKey(x => new { x.ComboId, x.MenuItemId });

        builder.HasOne(x => x.Combo)
               .WithMany(c => c.Items)
               .HasForeignKey(x => x.ComboId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.MenuItem)
               .WithMany()
               .HasForeignKey(x => x.MenuItemId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

using GoodBurger.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoodBurger.Tests.Handlers;

internal static class TestDbHelper
{
    public static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    public static async Task<(MenuItem sandwich, MenuItem potato, MenuItem drink)> SeedMenuAsync(AppDbContext ctx)
    {
        var catSandwich = ItemCategory.Create("Sanduíche");
        catSandwich.Id = 1;
        var catPotato = ItemCategory.Create("Batata");
        catPotato.Id = 2;
        var catDrink = ItemCategory.Create("Bebida");
        catDrink.Id = 3;

        ctx.ItemCategories.AddRange(catSandwich, catPotato, catDrink);
        await ctx.SaveChangesAsync();

        var sandwich = MenuItem.Create(1, "X Burger", 5.00m);
        var potato = MenuItem.Create(2, "Batata Frita", 2.00m);
        var drink = MenuItem.Create(3, "Refrigerante", 2.50m);

        ctx.MenuItems.AddRange(sandwich, potato, drink);
        await ctx.SaveChangesAsync();

        return (sandwich, potato, drink);
    }

    public static async Task SeedComboAsync(AppDbContext ctx, decimal discountPercentage, params Guid[] menuItemIds)
    {
        var combo = Combo.Create("Combo Teste", discountPercentage, "", menuItemIds);
        ctx.Combos.Add(combo);
        await ctx.SaveChangesAsync();
    }
}

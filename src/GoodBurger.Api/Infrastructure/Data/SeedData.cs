using GoodBurger.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoodBurger.Api.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAsync(IWebHostEnvironment environment, AppDbContext context)
    {
        await SeedItemCategoriesAsync(context);
        await SeedMenuItemsAsync(context);
        await SeedCombosAsync(context);
        await SeedUsersAsync(context, environment);
        await context.SaveChangesAsync();
    }

    private static async Task SeedItemCategoriesAsync(AppDbContext context)
    {
        if (await context.ItemCategories.AnyAsync())
            return;

        ItemCategory[] categories =
        [
            ItemCategory.Create("Sanduíche", "Itens de sanduíche"),
            ItemCategory.Create("Batata", "Acompanhamentos fritos"),
            ItemCategory.Create("Bebida", "Bebidas"),
        ];

        context.ItemCategories.AddRange(categories);
        await context.SaveChangesAsync();
    }

    private static async Task SeedMenuItemsAsync(AppDbContext context)
    {
        if (await context.MenuItems.AnyAsync())
            return;

        var categories = await context.ItemCategories.ToListAsync();

        var sanduicheCategory = categories.First(c => c.Name == "Sanduíche").Id;
        var batataCategory = categories.First(c => c.Name == "Batata").Id;
        var bebidaCategory = categories.First(c => c.Name == "Bebida").Id;

        MenuItem[] items =
        [
            MenuItem.Create(sanduicheCategory, "X Burger",     5.00m),
            MenuItem.Create(sanduicheCategory, "X Egg",        4.50m),
            MenuItem.Create(sanduicheCategory, "X Bacon",      7.00m),
            MenuItem.Create(batataCategory,    "Batata Frita", 2.00m),
            MenuItem.Create(bebidaCategory,    "Refrigerante", 2.50m),
        ];

        context.MenuItems.AddRange(items);
        await context.SaveChangesAsync();
    }

    private static async Task SeedCombosAsync(AppDbContext context)
    {
        if (await context.Combos.AnyAsync())
            return;

        var menuItems = await context.MenuItems.ToListAsync();
        var xBurger = menuItems.First(i => i.Name == "X Burger").Id;
        var batata = menuItems.First(i => i.Name == "Batata Frita").Id;
        var refrigerante = menuItems.First(i => i.Name == "Refrigerante").Id;

        Combo[] combos =
        [
            Combo.Create("Combo Completo", 20m, "X Burger + Batata Frita + Refrigerante",
                [xBurger, batata, refrigerante]),

            Combo.Create("Combo Sanduíche + Bebida", 15m, "X Burger + Refrigerante",
                [xBurger, refrigerante]),

            Combo.Create("Combo Sanduíche + Batata", 10m, "X Burger + Batata Frita",
                [xBurger, batata]),
        ];

        context.Combos.AddRange(combos);
    }

    private static async Task SeedUsersAsync(AppDbContext context, IWebHostEnvironment environment)
    {
        if (environment.IsProduction()) return;

        if (await context.Users.AnyAsync())
            return;

        string passwordHash = BCrypt.Net.BCrypt.HashPassword("admin123");
        context.Users.Add(User.Create("admin", passwordHash));
    }
}

using Bogus;
using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Domain.Entities;

namespace GoodBurger.Tests.Fakers;

internal static class MenuItemFaker
{
    private static readonly Faker _f = new("pt_BR");

    // Entity helpers (for seeding InMemory DB)
    public static MenuItem Sandwich(int categoryId = 1) =>
        MenuItem.Create(categoryId, _f.Commerce.ProductName(), _f.Random.Decimal(3m, 10m));

    public static MenuItem Potato(int categoryId = 2) =>
        MenuItem.Create(categoryId, "Batata Frita", 2.00m);

    public static MenuItem Drink(int categoryId = 3) =>
        MenuItem.Create(categoryId, "Refrigerante", 2.50m);

    // Snapshot helpers (for domain entity tests)
    public static MenuItemSnapshot SandwichSnapshot() =>
        new(Guid.NewGuid(), _f.Commerce.ProductName(), _f.Random.Decimal(3m, 10m), "Sanduíche");

    public static MenuItemSnapshot PotatoSnapshot() =>
        new(Guid.NewGuid(), "Batata Frita", 2.00m, "Batata");

    public static MenuItemSnapshot DrinkSnapshot() =>
        new(Guid.NewGuid(), "Refrigerante", 2.50m, "Bebida");
}

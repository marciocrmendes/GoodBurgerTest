using Bogus;
using GoodBurger.Api.Domain.Entities;

namespace GoodBurger.Tests.Fakers;

internal static class ComboFaker
{
    private static readonly Faker _f = new("pt_BR");

    public static Combo Create(decimal discountPercentage, params Guid[] itemIds) =>
        Combo.Create(_f.Commerce.ProductName(), discountPercentage, "", itemIds);
}

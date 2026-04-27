using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Domain.Entities;
using GoodBurger.Tests.Fakers;
using Shouldly;

namespace GoodBurger.Tests.Domain;

public class OrderTests
{
    [Fact]
    public void Create_WithItems_ReturnsSuccess()
    {
        var items = new[] { MenuItemFaker.SandwichSnapshot(), MenuItemFaker.DrinkSnapshot() };

        var result = Order.Create(items, 0);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Items.Count.ShouldBe(2);
    }

    [Fact]
    public void Create_WithNoItems_ReturnsFailure()
    {
        var result = Order.Create(Array.Empty<MenuItemSnapshot>(), 0);

        result.IsFailure.ShouldBeTrue();
        result.Error.Message.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Create_WithDiscount_CalculatesTotal()
    {
        var sandwich = new MenuItemSnapshot(Guid.NewGuid(), "X Burger", 5.00m, "Sanduíche");
        var potato   = new MenuItemSnapshot(Guid.NewGuid(), "Batata Frita", 2.00m, "Batata");
        var drink    = new MenuItemSnapshot(Guid.NewGuid(), "Refrigerante", 2.50m, "Bebida");

        var result = Order.Create([sandwich, potato, drink], 20m);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Subtotal.ShouldBe(9.50m);
        result.Value.DiscountPercentage.ShouldBe(20m);
        result.Value.DiscountAmount.ShouldBe(1.90m);
        result.Value.Total.ShouldBe(7.60m);
    }

    [Fact]
    public void Create_WithZeroDiscount_TotalEqualsSubtotal()
    {
        var items = new[] { MenuItemFaker.SandwichSnapshot(), MenuItemFaker.DrinkSnapshot() };

        var result = Order.Create(items, 0);

        result.IsSuccess.ShouldBeTrue();
        result.Value.DiscountAmount.ShouldBe(0m);
        result.Value.Total.ShouldBe(result.Value.Subtotal);
    }

    [Fact]
    public void Create_SnapshotsUnitPrice()
    {
        var item = new MenuItemSnapshot(Guid.NewGuid(), "X Burger", 5.00m, "Sanduíche");

        var result = Order.Create([item], 0);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Items.Single().UnitPrice.ShouldBe(5.00m);
    }

    [Fact]
    public void Update_WithItems_ReturnsSuccess()
    {
        var order = Order.Create([MenuItemFaker.SandwichSnapshot()], 0).Value;
        var newItems = new[] { MenuItemFaker.PotatoSnapshot(), MenuItemFaker.DrinkSnapshot() };

        var result = order.Update(newItems, 15m);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void Update_WithNoItems_ReturnsFailure()
    {
        var order = Order.Create([MenuItemFaker.SandwichSnapshot()], 0).Value;

        var result = order.Update(Array.Empty<MenuItemSnapshot>(), 0);

        result.IsFailure.ShouldBeTrue();
        result.Error.Message.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Update_ReplacesItemsAndRecalculates()
    {
        var order = Order.Create([MenuItemFaker.SandwichSnapshot()], 0).Value;
        var originalTotal = order.Total;

        var newItems = new[]
        {
            new MenuItemSnapshot(Guid.NewGuid(), "X Bacon", 7.00m, "Sanduíche"),
            new MenuItemSnapshot(Guid.NewGuid(), "Batata Frita", 2.00m, "Batata"),
        };
        order.Update(newItems, 10m);

        order.Items.Count.ShouldBe(2);
        order.Subtotal.ShouldBe(9.00m);
        order.DiscountPercentage.ShouldBe(10m);
        order.Total.ShouldNotBe(originalTotal);
    }
}

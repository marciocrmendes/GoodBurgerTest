using GoodBurger.Api.Domain.Entities;
using GoodBurger.Api.Features.Orders.CreateOrder;
using GoodBurger.Api.Infrastructure.Repositories;
using Shouldly;
using System.Net;

namespace GoodBurger.Tests.Handlers;

public class CreateOrderHandlerTests
{
    private static CreateOrderHandler BuildHandler(AppDbContext ctx) =>
        new(new MenuItemRepository(ctx), new ComboRepository(ctx), new OrderRepository(ctx), new UnitOfWork(ctx));

    [Fact]
    public async Task HandleAsync_SingleSandwich_ReturnsSuccess()
    {
        await using var ctx = TestDbHelper.CreateContext();
        var (sandwich, _, _) = await TestDbHelper.SeedMenuAsync(ctx);
        var handler = BuildHandler(ctx);

        var result = await handler.HandleAsync(new CreateOrderRequest([sandwich.Id]));

        result.IsSuccess.ShouldBeTrue();
        result.Value.Items.Count.ShouldBe(1);
    }

    [Fact]
    public async Task HandleAsync_MissingMenuItem_ReturnsNotFound()
    {
        await using var ctx = TestDbHelper.CreateContext();
        await TestDbHelper.SeedMenuAsync(ctx);
        var handler = BuildHandler(ctx);

        var result = await handler.HandleAsync(new CreateOrderRequest([Guid.NewGuid()]));

        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task HandleAsync_TwoSandwiches_ReturnsValidationError()
    {
        await using var ctx = TestDbHelper.CreateContext();
        var sandwich1 = MenuItem.Create(1, "X Burger", 5.00m);
        var sandwich2 = MenuItem.Create(1, "X Bacon", 7.00m);

        var cat = ItemCategory.Create("Sanduíche");
        cat.Id = 1;
        ctx.ItemCategories.Add(cat);
        ctx.MenuItems.AddRange(sandwich1, sandwich2);
        await ctx.SaveChangesAsync();

        var handler = BuildHandler(ctx);
        var result = await handler.HandleAsync(new CreateOrderRequest([sandwich1.Id, sandwich2.Id]));

        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task HandleAsync_TwoDrinks_ReturnsValidationError()
    {
        await using var ctx = TestDbHelper.CreateContext();
        var drink1 = MenuItem.Create(3, "Refrigerante", 2.50m);
        var drink2 = MenuItem.Create(3, "Suco", 3.00m);

        var cat = ItemCategory.Create("Bebida");
        cat.Id = 3;
        ctx.ItemCategories.Add(cat);
        ctx.MenuItems.AddRange(drink1, drink2);
        await ctx.SaveChangesAsync();

        var handler = BuildHandler(ctx);
        var result = await handler.HandleAsync(new CreateOrderRequest([drink1.Id, drink2.Id]));

        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task HandleAsync_TwoPotatoes_ReturnsValidationError()
    {
        await using var ctx = TestDbHelper.CreateContext();
        var p1 = MenuItem.Create(2, "Batata Frita", 2.00m);
        var p2 = MenuItem.Create(2, "Batata Rústica", 3.00m);

        var cat = ItemCategory.Create("Batata");
        cat.Id = 2;
        ctx.ItemCategories.Add(cat);
        ctx.MenuItems.AddRange(p1, p2);
        await ctx.SaveChangesAsync();

        var handler = BuildHandler(ctx);
        var result = await handler.HandleAsync(new CreateOrderRequest([p1.Id, p2.Id]));

        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task HandleAsync_ComboMatch_AppliesDiscount()
    {
        await using var ctx = TestDbHelper.CreateContext();
        var (sandwich, potato, drink) = await TestDbHelper.SeedMenuAsync(ctx);
        await TestDbHelper.SeedComboAsync(ctx, 20m, sandwich.Id, potato.Id, drink.Id);

        var handler = BuildHandler(ctx);
        var result = await handler.HandleAsync(
            new CreateOrderRequest([sandwich.Id, potato.Id, drink.Id]));

        result.IsSuccess.ShouldBeTrue();
        result.Value.DiscountPercentage.ShouldBe(20m);
        result.Value.DiscountAmount.ShouldBeGreaterThan(0m);
    }

    [Fact]
    public async Task HandleAsync_NoComboMatch_ZeroDiscount()
    {
        await using var ctx = TestDbHelper.CreateContext();
        var (sandwich, potato, _) = await TestDbHelper.SeedMenuAsync(ctx);

        var handler = BuildHandler(ctx);
        var result = await handler.HandleAsync(
            new CreateOrderRequest([sandwich.Id, potato.Id]));

        result.IsSuccess.ShouldBeTrue();
        result.Value.DiscountPercentage.ShouldBe(0m);
    }

    [Fact]
    public async Task HandleAsync_PartialComboItems_ZeroDiscount()
    {
        await using var ctx = TestDbHelper.CreateContext();
        var (sandwich, potato, drink) = await TestDbHelper.SeedMenuAsync(ctx);
        await TestDbHelper.SeedComboAsync(ctx, 20m, sandwich.Id, potato.Id, drink.Id);

        var handler = BuildHandler(ctx);
        var result = await handler.HandleAsync(
            new CreateOrderRequest([sandwich.Id, potato.Id]));

        result.IsSuccess.ShouldBeTrue();
        result.Value.DiscountPercentage.ShouldBe(0m);
    }
}

using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Domain.Entities;
using GoodBurger.Api.Features.Orders.UpdateOrder;
using GoodBurger.Api.Infrastructure.Repositories;
using Shouldly;
using System.Net;

namespace GoodBurger.Tests.Handlers;

public class UpdateOrderHandlerTests
{
    private static UpdateOrderHandler BuildHandler(AppDbContext ctx) =>
        new(new OrderRepository(ctx), new MenuItemRepository(ctx), new ComboRepository(ctx), new UnitOfWork(ctx));

    private static async Task<Order> SeedOrderAsync(AppDbContext ctx, params MenuItemSnapshot[] snapshots)
    {
        var orderResult = Order.Create(snapshots, 0);
        ctx.Orders.Add(orderResult.Value);
        await ctx.SaveChangesAsync();
        return orderResult.Value;
    }

    [Fact]
    public async Task HandleAsync_NonExistentOrder_ReturnsNotFound()
    {
        await using var ctx = TestDbHelper.CreateContext();
        await TestDbHelper.SeedMenuAsync(ctx);
        var handler = BuildHandler(ctx);

        var result = await handler.HandleAsync(
            new UpdateOrderRequest(Guid.NewGuid(), [Guid.NewGuid()]));

        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task HandleAsync_TwoSandwiches_ReturnsValidationError()
    {
        await using var ctx = TestDbHelper.CreateContext();
        var s1 = MenuItem.Create(1, "X Burger", 5.00m);
        var s2 = MenuItem.Create(1, "X Bacon", 7.00m);
        var cat = ItemCategory.Create("Sanduíche");
        cat.Id = 1;
        ctx.ItemCategories.Add(cat);
        ctx.MenuItems.AddRange(s1, s2);
        await ctx.SaveChangesAsync();

        var order = await SeedOrderAsync(ctx, new MenuItemSnapshot(s1.Id, s1.Name, s1.Price, "Sanduíche"));
        var handler = BuildHandler(ctx);

        var result = await handler.HandleAsync(
            new UpdateOrderRequest(order.Id, [s1.Id, s2.Id]));

        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task HandleAsync_ComboMatch_AppliesDiscount()
    {
        await using var ctx = TestDbHelper.CreateContext();
        var (sandwich, potato, drink) = await TestDbHelper.SeedMenuAsync(ctx);
        await TestDbHelper.SeedComboAsync(ctx, 20m, sandwich.Id, potato.Id, drink.Id);

        var order = await SeedOrderAsync(ctx, new MenuItemSnapshot(sandwich.Id, sandwich.Name, sandwich.Price, "Sanduíche"));
        var handler = BuildHandler(ctx);

        var result = await handler.HandleAsync(
            new UpdateOrderRequest(order.Id, [sandwich.Id, potato.Id, drink.Id]));

        result.IsSuccess.ShouldBeTrue();
        result.Value.DiscountPercentage.ShouldBe(20m);
    }

    [Fact]
    public async Task HandleAsync_NoComboMatch_ZeroDiscount()
    {
        await using var ctx = TestDbHelper.CreateContext();
        var (sandwich, potato, _) = await TestDbHelper.SeedMenuAsync(ctx);

        var order = await SeedOrderAsync(ctx, new MenuItemSnapshot(sandwich.Id, sandwich.Name, sandwich.Price, "Sanduíche"));
        var handler = BuildHandler(ctx);

        var result = await handler.HandleAsync(
            new UpdateOrderRequest(order.Id, [sandwich.Id, potato.Id]));

        result.IsSuccess.ShouldBeTrue();
        result.Value.DiscountPercentage.ShouldBe(0m);
    }
}

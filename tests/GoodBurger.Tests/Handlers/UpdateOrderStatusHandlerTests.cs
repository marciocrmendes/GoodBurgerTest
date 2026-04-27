using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Domain.Enums;
using GoodBurger.Api.Features.Orders.UpdateOrderStatus;
using GoodBurger.Api.Infrastructure.Repositories;
using Shouldly;
using System.Net;

namespace GoodBurger.Tests.Handlers;

public class UpdateOrderStatusHandlerTests
{
    private static UpdateOrderStatusHandler BuildHandler(AppDbContext ctx) =>
        new(new OrderRepository(ctx), new UnitOfWork(ctx));

    private static async Task<Guid> SeedOrderAsync(AppDbContext ctx)
    {
        var (sandwich, _, _) = await TestDbHelper.SeedMenuAsync(ctx);
        var menuItemRepo = new MenuItemRepository(ctx);
        var comboRepo = new ComboRepository(ctx);
        var orderRepo = new OrderRepository(ctx);
        var uow = new UnitOfWork(ctx);

        var snapshot = new MenuItemSnapshot(sandwich.Id, sandwich.Name, sandwich.Price, "Sanduíche");
        var order = GoodBurger.Api.Domain.Entities.Order.Create([snapshot], 0).Value;
        ctx.Orders.Add(order);
        await ctx.SaveChangesAsync();
        return order.Id;
    }

    [Fact]
    public async Task HandleAsync_NonExistentOrder_ReturnsNotFound()
    {
        await using var ctx = TestDbHelper.CreateContext();
        var handler = BuildHandler(ctx);

        var result = await handler.HandleAsync(new UpdateOrderStatusRequest(Guid.NewGuid()));

        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task HandleAsync_ToDoOrder_AdvancesToInProgress()
    {
        await using var ctx = TestDbHelper.CreateContext();
        var orderId = await SeedOrderAsync(ctx);
        var handler = BuildHandler(ctx);

        var result = await handler.HandleAsync(new UpdateOrderStatusRequest(orderId));

        result.IsSuccess.ShouldBeTrue();
        result.Value.Status.ShouldBe(OrderStatus.InProgress);
    }

    [Fact]
    public async Task HandleAsync_FullAdvanceCycle_ReachesDelivered()
    {
        await using var ctx = TestDbHelper.CreateContext();
        var orderId = await SeedOrderAsync(ctx);
        var handler = BuildHandler(ctx);

        await handler.HandleAsync(new UpdateOrderStatusRequest(orderId)); // → InProgress
        await handler.HandleAsync(new UpdateOrderStatusRequest(orderId)); // → Done
        var result = await handler.HandleAsync(new UpdateOrderStatusRequest(orderId)); // → Delivered

        result.IsSuccess.ShouldBeTrue();
        result.Value.Status.ShouldBe(OrderStatus.Delivered);
    }

    [Fact]
    public async Task HandleAsync_DeliveredOrder_ReturnsValidationError()
    {
        await using var ctx = TestDbHelper.CreateContext();
        var orderId = await SeedOrderAsync(ctx);
        var handler = BuildHandler(ctx);

        await handler.HandleAsync(new UpdateOrderStatusRequest(orderId));
        await handler.HandleAsync(new UpdateOrderStatusRequest(orderId));
        await handler.HandleAsync(new UpdateOrderStatusRequest(orderId));

        var result = await handler.HandleAsync(new UpdateOrderStatusRequest(orderId));

        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe(HttpStatusCode.BadRequest);
    }
}

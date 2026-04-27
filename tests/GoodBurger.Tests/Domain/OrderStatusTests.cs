using GoodBurger.Api.Domain.Entities;
using GoodBurger.Api.Domain.Enums;
using GoodBurger.Tests.Fakers;
using Shouldly;

namespace GoodBurger.Tests.Domain;

public class OrderStatusTests
{
    private static Order NewOrder() =>
        Order.Create([MenuItemFaker.SandwichSnapshot()], 0).Value;

    [Fact]
    public void NewOrder_HasToDoStatus()
    {
        var order = NewOrder();
        order.Status.ShouldBe(OrderStatus.ToDo);
    }

    [Fact]
    public void AdvanceStatus_FromToDo_BecomesInProgress()
    {
        var order = NewOrder();

        var result = order.AdvanceStatus();

        result.IsSuccess.ShouldBeTrue();
        order.Status.ShouldBe(OrderStatus.InProgress);
    }

    [Fact]
    public void AdvanceStatus_FromInProgress_BecomesDone()
    {
        var order = NewOrder();
        order.AdvanceStatus();

        var result = order.AdvanceStatus();

        result.IsSuccess.ShouldBeTrue();
        order.Status.ShouldBe(OrderStatus.Done);
    }

    [Fact]
    public void AdvanceStatus_FromDone_BecomesDelivered()
    {
        var order = NewOrder();
        order.AdvanceStatus();
        order.AdvanceStatus();

        var result = order.AdvanceStatus();

        result.IsSuccess.ShouldBeTrue();
        order.Status.ShouldBe(OrderStatus.Delivered);
    }

    [Fact]
    public void AdvanceStatus_FromDelivered_ReturnsFailure()
    {
        var order = NewOrder();
        order.AdvanceStatus();
        order.AdvanceStatus();
        order.AdvanceStatus();

        var result = order.AdvanceStatus();

        result.IsFailure.ShouldBeTrue();
        order.Status.ShouldBe(OrderStatus.Delivered);
    }

    [Fact]
    public void AdvanceStatus_UpdatesUpdatedAt()
    {
        var order = NewOrder();
        var before = order.UpdatedAt;

        order.AdvanceStatus();

        order.UpdatedAt.ShouldNotBe(before);
    }
}

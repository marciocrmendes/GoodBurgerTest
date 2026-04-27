using GoodBurger.Api.Domain.Abstractions;
using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Domain.Enums;

namespace GoodBurger.Api.Domain.Entities;

public class Order : BaseEntity<Guid>
{
    private Order() { }

    public decimal Subtotal { get; private set; }
    public decimal DiscountPercentage { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal Total { get; private set; }
    public OrderStatus Status { get; private set; }

    private readonly List<OrderItem> _items = [];
    public virtual IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public static Result<Order> Create(IEnumerable<MenuItemSnapshot> items, decimal discountPercentage)
    {
        var list = items.ToList();
        if (list.Count == 0)
            return Result.Failure<Order>(Error.Validation("O pedido deve conter ao menos um item."));

        var order = new Order
        {
            Id = Guid.NewGuid(),
            Status = OrderStatus.ToDo,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        foreach (var item in list)
            order._items.Add(OrderItem.Create(order.Id, item));

        order.RecalculateValues(discountPercentage);
        return Result.Success(order);
    }

    public Result Update(IEnumerable<MenuItemSnapshot> items, decimal discountPercentage)
    {
        var list = items.ToList();
        if (list.Count == 0)
            return Result.Failure(Error.Validation("O pedido deve conter ao menos um item."));

        _items.Clear();
        foreach (var item in list)
            _items.Add(OrderItem.Create(Id, item));

        UpdatedAt = DateTime.UtcNow;
        RecalculateValues(discountPercentage);
        return Result.Success();
    }

    public Result AdvanceStatus()
    {
        if (Status == OrderStatus.Delivered)
            return Result.Failure(Error.Validation("O pedido já foi entregue."));

        Status = (OrderStatus)((int)Status + 1);
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    private void RecalculateValues(decimal discountPercentage)
    {
        Subtotal = _items.Sum(i => i.UnitPrice);
        DiscountPercentage = discountPercentage;
        DiscountAmount = Math.Round(Subtotal * (discountPercentage / 100), 2);
        Total = Subtotal - DiscountAmount;
    }
}

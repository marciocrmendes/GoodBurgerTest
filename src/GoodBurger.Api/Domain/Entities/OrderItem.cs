using GoodBurger.Api.Domain.Abstractions;
using GoodBurger.Api.Domain.Common;

namespace GoodBurger.Api.Domain.Entities;

public class OrderItem : BaseEntity
{
    private OrderItem() { }

    public Guid OrderId { get; private set; }
    public Guid MenuItemId { get; private set; }
    public decimal UnitPrice { get; private set; }

    public virtual Order Order { get; private set; } = null!;
    public virtual MenuItem MenuItem { get; private set; } = null!;

    internal static OrderItem Create(Guid orderId, MenuItemSnapshot snapshot)
        => new()
        {
            OrderId = orderId,
            MenuItemId = snapshot.Id,
            UnitPrice = snapshot.Price
        };
}

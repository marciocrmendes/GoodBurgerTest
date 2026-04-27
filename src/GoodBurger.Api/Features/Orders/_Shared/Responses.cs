using GoodBurger.Api.Domain.Enums;

namespace GoodBurger.Api.Features.Orders._Shared;

public record OrderItemResponse(Guid MenuItemId, string Name, decimal UnitPrice);

public record OrderResponse(
    Guid Id,
    OrderStatus Status,
    List<OrderItemResponse> Items,
    decimal Subtotal,
    decimal DiscountPercentage,
    decimal DiscountAmount,
    decimal Total,
    DateTime CreatedAt,
    DateTime UpdatedAt);

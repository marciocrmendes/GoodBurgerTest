namespace GoodBurger.Api.Features.Orders.CreateOrder;

public record CreateOrderRequest(List<Guid> MenuItemIds);

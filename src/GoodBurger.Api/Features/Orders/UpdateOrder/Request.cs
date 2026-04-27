namespace GoodBurger.Api.Features.Orders.UpdateOrder;

public record UpdateOrderRequest(Guid Id, List<Guid> MenuItemIds);

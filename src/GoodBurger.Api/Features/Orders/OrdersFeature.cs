using GoodBurger.Api.Features.Orders.CreateOrder;
using GoodBurger.Api.Features.Orders.DeleteOrder;
using GoodBurger.Api.Features.Orders.GetOrderById;
using GoodBurger.Api.Features.Orders.GetOrders;
using GoodBurger.Api.Features.Orders.UpdateOrder;
using GoodBurger.Api.Features.Orders.UpdateOrderStatus;

namespace GoodBurger.Api.Features.Orders;

public static class OrdersFeature
{
    public static IEndpointRouteBuilder MapOrdersFeature(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/orders")
            .WithTags("Pedidos")
            .RequireAuthorization();

        group.MapGetOrders();
        group.MapGetOrderById();
        group.MapCreateOrder();
        group.MapUpdateOrder();
        group.MapUpdateOrderStatus();
        group.MapDeleteOrder();

        return app;
    }
}

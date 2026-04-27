using GoodBurger.Api.Features.Orders._Shared;

namespace GoodBurger.Api.Features.Orders.GetOrders;

public static class GetOrdersEndpoint
{
    public static RouteGroupBuilder MapGetOrders(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (GetOrdersHandler handler, CancellationToken stoppingToken) =>
        {
            var result = await handler.HandleAsync(new GetOrdersRequest(), stoppingToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.Problem(detail: result.Error.Message, statusCode: (int)result.Error.Code);
        })
        .WithName("GetOrders")
        .WithSummary("Listar pedidos")
        .Produces<List<OrderResponse>>()
        .ProducesProblem(500);

        return group;
    }
}

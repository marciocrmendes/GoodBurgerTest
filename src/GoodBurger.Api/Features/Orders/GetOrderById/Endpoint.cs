using GoodBurger.Api.Features.Orders._Shared;

namespace GoodBurger.Api.Features.Orders.GetOrderById;

public static class GetOrderByIdEndpoint
{
    public static RouteGroupBuilder MapGetOrderById(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", async (
            Guid id,
            GetOrderByIdHandler handler,
            CancellationToken stoppingToken) =>
        {
            var result = await handler.HandleAsync(new GetOrderByIdRequest(id), stoppingToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.Problem(detail: result.Error.Message, statusCode: (int)result.Error.Code);
        })
        .WithName("GetOrderById")
        .WithSummary("Consultar pedido por ID")
        .Produces<OrderResponse>()
        .ProducesProblem(404)
        .ProducesProblem(500);

        return group;
    }
}

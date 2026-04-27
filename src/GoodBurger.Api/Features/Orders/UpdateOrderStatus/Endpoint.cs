using GoodBurger.Api.Features.Orders._Shared;

namespace GoodBurger.Api.Features.Orders.UpdateOrderStatus;

public static class UpdateOrderStatusEndpoint
{
    public static RouteGroupBuilder MapUpdateOrderStatus(this RouteGroupBuilder group)
    {
        group.MapPatch("/{id:guid}/status", async (
            Guid id,
            UpdateOrderStatusHandler handler,
            CancellationToken stoppingToken) =>
        {
            var result = await handler.HandleAsync(new UpdateOrderStatusRequest(id), stoppingToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.Problem(detail: result.Error.Message, statusCode: (int)result.Error.Code);
        })
        .WithName("UpdateOrderStatus")
        .WithSummary("Avançar status do pedido")
        .WithDescription("Avança o status sequencialmente: ToDo → InProgress → Done → Delivered")
        .Produces<OrderResponse>()
        .ProducesProblem(404)
        .ProducesProblem(422);

        return group;
    }
}

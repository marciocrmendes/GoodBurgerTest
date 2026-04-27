namespace GoodBurger.Api.Features.Orders.DeleteOrder;

public static class DeleteOrderEndpoint
{
    public static RouteGroupBuilder MapDeleteOrder(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:guid}", async (
            Guid id,
            DeleteOrderHandler handler,
            CancellationToken stoppingToken) =>
        {
            var result = await handler.HandleAsync(new DeleteOrderRequest(id), stoppingToken);

            return result.IsSuccess ?
                Results.NoContent() :
                Results.Problem(detail: result.Error.Message, statusCode: (int)result.Error.Code);
        })
        .WithName("DeleteOrder")
        .WithSummary("Remover pedido")
        .Produces(204)
        .ProducesProblem(404)
        .ProducesProblem(500);

        return group;
    }
}

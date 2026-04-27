namespace GoodBurger.Api.Features.MenuItems.DeleteMenuItem;

internal static class DeleteMenuItemEndpoint
{
    internal static void MapDeleteMenuItemEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id:guid}", async (
            Guid id,
            DeleteMenuItemHandler handler,
            CancellationToken stoppingToken) =>
        {
            var result = await handler.HandleAsync(new DeleteMenuItemRequest(id), stoppingToken);

            return result.IsSuccess
                ? Results.NoContent()
                : Results.Problem(detail: result.Error.Message, statusCode: (int)result.Error.Code);
        })
        .WithName("DeleteMenuItem")
        .WithDescription("Remove um item do cardápio")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(404);
    }
}

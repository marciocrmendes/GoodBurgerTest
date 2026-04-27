using GoodBurger.Api.Features.MenuItems._Shared;

namespace GoodBurger.Api.Features.MenuItems.GetMenuItemById;

internal static class GetMenuItemByIdEndpoint
{
    internal static void MapGetMenuItemByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{id:guid}", async (
            Guid id,
            GetMenuItemByIdHandler handler,
            CancellationToken stoppingToken) =>
        {
            var result = await handler.HandleAsync(new GetMenuItemByIdRequest(id), stoppingToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.Problem(detail: result.Error.Message, statusCode: (int)result.Error.Code);
        })
        .WithName("GetMenuItemById")
        .WithDescription("Busca um item do cardápio por ID")
        .Produces<MenuItemResponse>()
        .ProducesProblem(404);
    }
}

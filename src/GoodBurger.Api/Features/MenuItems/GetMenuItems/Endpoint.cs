using GoodBurger.Api.Features.MenuItems._Shared;

namespace GoodBurger.Api.Features.MenuItems.GetMenuItems;

internal static class GetMenuItemsEndpoint
{
    internal static void MapGetMenuItemsEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (
            GetMenuItemsHandler handler,
            CancellationToken stoppingToken) =>
        {
            var result = await handler.HandleAsync(new GetMenuItemsRequest(), stoppingToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.Problem(detail: result.Error.Message, statusCode: (int)result.Error.Code);
        })
        .WithName("GetMenuItems")
        .WithDescription("Lista todos os itens do cardápio")
        .Produces<List<MenuItemResponse>>();
    }
}

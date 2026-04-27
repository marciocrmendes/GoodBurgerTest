namespace GoodBurger.Api.Features.Menu;

public static class MenuEndpoints
{
    public static IEndpointRouteBuilder MapMenuEndpoints(this IEndpointRouteBuilder app)
    {
        IEndpointRouteBuilder group = app.MapGroup("/menu").WithTags("Cardápio");

        group.MapGet("/", async (GetMenuHandler handler, CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new GetMenuRequest(), cancellationToken);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.Problem(detail: result.Error.Message, statusCode: 500);
        })
        .WithName("GetMenu")
        .WithSummary("Listar cardápio")
        .WithDescription("Retorna todos os itens disponíveis com nome e preço.")
        .Produces<List<MenuItemDto>>()
        .AllowAnonymous();

        return app;
    }
}

namespace GoodBurger.Api.Features.Combos.GetCombos;

internal static class GetCombosEndpoint
{
    internal static void MapGetCombosEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (
            GetCombosHandler handler,
            CancellationToken stoppingToken) =>
        {
            var result = await handler.HandleAsync(new GetCombosRequest(), stoppingToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.Problem(detail: result.Error.Message, statusCode: (int)result.Error.Code);
        })
        .WithName("GetCombos")
        .WithDescription("Lista todos os combos disponíveis");
    }
}

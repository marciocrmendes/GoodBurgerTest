namespace GoodBurger.Api.Features.Combos.DeleteCombo;

internal static class DeleteComboEndpoint
{
    internal static void MapDeleteComboEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/{comboId}", async (
            Guid comboId,
            DeleteComboHandler handler,
            CancellationToken stoppingToken) =>
            {
                var result = await handler.HandleAsync(new DeleteComboRequest(comboId), stoppingToken);

                return result.IsSuccess
                    ? Results.NoContent()
                    : Results.NotFound(result.Error);
            })
            .WithName("DeleteCombo")
            .WithDescription("Remove um combo");
    }
}

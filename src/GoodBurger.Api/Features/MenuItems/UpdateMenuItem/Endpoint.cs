using FluentValidation;
using GoodBurger.Api.Features.MenuItems._Shared;
using Microsoft.AspNetCore.Mvc;

namespace GoodBurger.Api.Features.MenuItems.UpdateMenuItem;

internal static class UpdateMenuItemEndpoint
{
    internal static void MapUpdateMenuItemEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPut("/{id:guid}", async (
            Guid id,
            [FromBody] UpdateMenuItemRequest request,
            UpdateMenuItemHandler handler,
            IValidator<UpdateMenuItemRequest> validator,
            CancellationToken stoppingToken) =>
        {
            var validation = await validator.ValidateAsync(request, stoppingToken);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var result = await handler.HandleAsync(request with { Id = id }, stoppingToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.Problem(detail: result.Error.Message, statusCode: (int)result.Error.Code);
        })
        .WithName("UpdateMenuItem")
        .WithDescription("Atualiza um item do cardápio")
        .Produces<MenuItemResponse>()
        .ProducesValidationProblem()
        .ProducesProblem(404);
    }
}

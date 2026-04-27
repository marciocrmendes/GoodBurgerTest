using FluentValidation;
using GoodBurger.Api.Features.MenuItems._Shared;
using Microsoft.AspNetCore.Mvc;

namespace GoodBurger.Api.Features.MenuItems.CreateMenuItem;

internal static class CreateMenuItemEndpoint
{
    internal static void MapCreateMenuItemEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (
            [FromBody] CreateMenuItemRequest request,
            CreateMenuItemHandler handler,
            IValidator<CreateMenuItemRequest> validator,
            CancellationToken stoppingToken) =>
        {
            var validation = await validator.ValidateAsync(request, stoppingToken);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var result = await handler.HandleAsync(request, stoppingToken);

            return result.IsSuccess
                ? Results.Created($"/menu-items/{result.Value.Id}", result.Value)
                : Results.Problem(detail: result.Error.Message, statusCode: (int)result.Error.Code);
        })
        .WithName("CreateMenuItem")
        .WithDescription("Cadastra um novo item no cardápio")
        .Produces<MenuItemResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(404);
    }
}

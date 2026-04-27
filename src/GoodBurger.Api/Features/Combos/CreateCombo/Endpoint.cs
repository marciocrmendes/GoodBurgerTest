using FluentValidation;
using GoodBurger.Api.Features.Combos._Shared;
using Microsoft.AspNetCore.Mvc;

namespace GoodBurger.Api.Features.Combos.CreateCombo;

internal static class CreateComboEndpoint
{
    internal static void MapCreateComboEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (
            [FromBody] CreateComboRequest request,
            CreateComboHandler handler,
            IValidator<CreateComboRequest> validator,
            CancellationToken stoppingToken) =>
        {
            var validation = await validator.ValidateAsync(request, stoppingToken);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var result = await handler.HandleAsync(request, stoppingToken);

            return result.IsSuccess
                ? Results.Created($"/combos/{result.Value.Id}", result.Value)
                : Results.BadRequest(result.Error);
        })
            .WithName("CreateCombo")
            .WithDescription("Cria um novo combo com desconto")
            .Produces<ComboResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
    }
}

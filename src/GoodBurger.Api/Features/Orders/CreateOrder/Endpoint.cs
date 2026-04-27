using FluentValidation;
using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Features.Orders._Shared;
using Microsoft.AspNetCore.Mvc;

namespace GoodBurger.Api.Features.Orders.CreateOrder;

public static class CreateOrderEndpoint
{
    public static RouteGroupBuilder MapCreateOrder(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (
            [FromBody] CreateOrderRequest request,
            IValidator<CreateOrderRequest> validator,
            CreateOrderHandler handler,
            CancellationToken stoppingToken) =>
        {
            var validation = await validator.ValidateAsync(request, stoppingToken);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            Result<OrderResponse> result = await handler.HandleAsync(request, stoppingToken);

            return result.IsSuccess
                ? Results.Created($"/orders/{result.Value.Id}", result.Value)
                : Results.Problem(detail: result.Error.Message, statusCode: (int)result.Error.Code);
        })
        .WithName("CreateOrder")
        .WithSummary("Criar pedido")
        .Produces<OrderResponse>(201)
        .ProducesValidationProblem()
        .ProducesProblem(404)
        .ProducesProblem(422)
        .ProducesProblem(500);

        return group;
    }
}

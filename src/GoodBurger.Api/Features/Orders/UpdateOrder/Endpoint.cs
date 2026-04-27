using FluentValidation;
using GoodBurger.Api.Features.Orders._Shared;
using Microsoft.AspNetCore.Mvc;

namespace GoodBurger.Api.Features.Orders.UpdateOrder;

public static class UpdateOrderEndpoint
{
    public static RouteGroupBuilder MapUpdateOrder(this RouteGroupBuilder group)
    {
        group.MapPut("/{id:guid}", async (
            Guid id,
            [FromBody] UpdateOrderRequest request,
            IValidator<UpdateOrderRequest> validator,
            UpdateOrderHandler handler,
            CancellationToken stoppingToken) =>
        {
            var validation = await validator.ValidateAsync(request, stoppingToken);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var result = await handler.HandleAsync(new(id, request.MenuItemIds), stoppingToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.Problem(detail: result.Error.Message, statusCode: (int)result.Error.Code);
        })
        .WithName("UpdateOrder")
        .WithSummary("Atualizar pedido")
        .Produces<OrderResponse>()
        .ProducesValidationProblem()
        .ProducesProblem(404)
        .ProducesProblem(422)
        .ProducesProblem(500);

        return group;
    }
}

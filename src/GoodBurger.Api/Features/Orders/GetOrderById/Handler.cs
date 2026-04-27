using GoodBurger.Api.Domain.Abstractions;
using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Features.Orders._Shared;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;

namespace GoodBurger.Api.Features.Orders.GetOrderById;

public class GetOrderByIdHandler(IOrderRepository orderRepository) : IDomainHandler<GetOrderByIdRequest, OrderResponse>
{
    public async Task<Result<OrderResponse>> HandleAsync(GetOrderByIdRequest request, CancellationToken stoppingToken = default)
    {
        var order = await orderRepository.GetByIdAsync(request.Id, stoppingToken);
        return order is null ? Result.Failure<OrderResponse>(Error.NotFound("Pedido não encontrado.")) : Result.Success(order);
    }
}

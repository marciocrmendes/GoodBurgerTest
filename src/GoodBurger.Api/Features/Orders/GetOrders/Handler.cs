using GoodBurger.Api.Domain.Abstractions;
using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Features.Orders._Shared;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;

namespace GoodBurger.Api.Features.Orders.GetOrders;

public class GetOrdersHandler(IOrderRepository orderRepository) : IDomainHandler<GetOrdersRequest, List<OrderResponse>>
{
    public async Task<Result<List<OrderResponse>>> HandleAsync(GetOrdersRequest request, CancellationToken stoppingToken = default)
        => Result.Success(await orderRepository.GetAllAsync(stoppingToken));
}

using GoodBurger.Api.Domain.Abstractions;
using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Features.Orders._Shared;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;

namespace GoodBurger.Api.Features.Orders.UpdateOrderStatus;

public class UpdateOrderStatusHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : IDomainHandler<UpdateOrderStatusRequest, OrderResponse>
{
    public async Task<Result<OrderResponse>> HandleAsync(UpdateOrderStatusRequest request, CancellationToken stoppingToken = default)
    {
        var order = await orderRepository.FindTrackedAsync(request.Id, stoppingToken);
        if (order is null)
            return Result.Failure<OrderResponse>(Error.NotFound("Pedido não encontrado."));

        var advanceResult = order.AdvanceStatus();
        if (advanceResult.IsFailure)
            return Result.Failure<OrderResponse>(advanceResult.Error);

        await unitOfWork.CommitAsync(stoppingToken);

        return Result.Success(await orderRepository.ReloadAsResponseAsync(request.Id, stoppingToken));
    }
}

using GoodBurger.Api.Domain.Abstractions;
using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;
using static GoodBurger.Api.Domain.Common.Result;

namespace GoodBurger.Api.Features.Orders.DeleteOrder;

public class DeleteOrderHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : IDomainHandler<DeleteOrderRequest, Empty>
{
    public async Task<Result<Empty>> HandleAsync(DeleteOrderRequest request, CancellationToken stoppingToken = default)
    {
        var order = await orderRepository.FindTrackedAsync(request.Id, stoppingToken);
        if (order is null)
            return Failure<Empty>(Error.NotFound("Pedido não encontrado."));

        orderRepository.Remove(order);
        await unitOfWork.CommitAsync(stoppingToken);
        return Success<Empty>();
    }
}

using GoodBurger.Api.Domain.Entities;
using GoodBurger.Api.Features.Orders._Shared;

namespace GoodBurger.Api.Infrastructure.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<OrderResponse?> GetByIdAsync(Guid id, CancellationToken stoppingToken = default);
    Task<List<OrderResponse>> GetAllAsync(CancellationToken stoppingToken = default);
    Task<Order?> FindTrackedWithItemsAsync(Guid id, CancellationToken stoppingToken = default);
    Task<Order?> FindTrackedAsync(Guid id, CancellationToken stoppingToken = default);
    Task<OrderResponse> ReloadAsResponseAsync(Guid id, CancellationToken stoppingToken = default);
    void Add(Order order);
    void Remove(Order order);
}

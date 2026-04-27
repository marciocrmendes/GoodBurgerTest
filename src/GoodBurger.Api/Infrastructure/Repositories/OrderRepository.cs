using GoodBurger.Api.Domain.Entities;
using GoodBurger.Api.Features.Orders._Shared;
using GoodBurger.Api.Infrastructure.Data;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoodBurger.Api.Infrastructure.Repositories;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public Task<OrderResponse?> GetByIdAsync(Guid id, CancellationToken stoppingToken = default)
        => context.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .ThenInclude(i => i.MenuItem)
            .Where(o => o.Id == id)
            .Select(o => new OrderResponse(
                o.Id,
                o.Status,
                o.Items.Select(i => new OrderItemResponse(i.MenuItemId, i.MenuItem.Name, i.UnitPrice)).ToList(),
                o.Subtotal,
                o.DiscountPercentage,
                o.DiscountAmount,
                o.Total,
                o.CreatedAt,
                o.UpdatedAt ?? o.CreatedAt))
            .FirstOrDefaultAsync(stoppingToken);

    public Task<List<OrderResponse>> GetAllAsync(CancellationToken stoppingToken = default)
        => context.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .ThenInclude(i => i.MenuItem)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new OrderResponse(
                o.Id,
                o.Status,
                o.Items.Select(i => new OrderItemResponse(i.MenuItemId, i.MenuItem.Name, i.UnitPrice)).ToList(),
                o.Subtotal,
                o.DiscountPercentage,
                o.DiscountAmount,
                o.Total,
                o.CreatedAt,
                o.UpdatedAt ?? o.CreatedAt))
            .ToListAsync(stoppingToken);

    public Task<Order?> FindTrackedWithItemsAsync(Guid id, CancellationToken stoppingToken = default)
        => context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, stoppingToken);

    public Task<Order?> FindTrackedAsync(Guid id, CancellationToken stoppingToken = default)
        => context.Orders
            .FirstOrDefaultAsync(o => o.Id == id, stoppingToken);

    public Task<OrderResponse> ReloadAsResponseAsync(Guid id, CancellationToken stoppingToken = default)
        => context.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .ThenInclude(i => i.MenuItem)
            .Where(o => o.Id == id)
            .Select(o => new OrderResponse(
                o.Id,
                o.Status,
                o.Items.Select(i => new OrderItemResponse(i.MenuItemId, i.MenuItem.Name, i.UnitPrice)).ToList(),
                o.Subtotal,
                o.DiscountPercentage,
                o.DiscountAmount,
                o.Total,
                o.CreatedAt,
                o.UpdatedAt ?? o.CreatedAt))
            .FirstAsync(stoppingToken);

    public void Add(Order order) => context.Orders.Add(order);

    public void Remove(Order order) => context.Orders.Remove(order);
}

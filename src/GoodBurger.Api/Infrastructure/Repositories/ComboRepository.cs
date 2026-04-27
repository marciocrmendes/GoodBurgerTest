using GoodBurger.Api.Domain.Entities;
using GoodBurger.Api.Features.Combos._Shared;
using GoodBurger.Api.Infrastructure.Data;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoodBurger.Api.Infrastructure.Repositories;

public class ComboRepository(AppDbContext context) : IComboRepository
{
    public Task<List<ComboResponse>> GetAllAsync(CancellationToken stoppingToken = default)
        => context.Combos
            .AsNoTracking()
            .Include(c => c.Items)
            .ThenInclude(i => i.MenuItem)
            .OrderBy(c => c.Name)
            .Select(c => new ComboResponse(
                c.Id,
                c.Name,
                c.Description,
                c.DiscountPercentage,
                c.Items.Select(i => new ComboItemResponse(i.MenuItem.Id, i.MenuItem.Name, i.MenuItem.Price)).ToList(),
                c.CreatedAt,
                c.UpdatedAt ?? c.CreatedAt))
            .ToListAsync(stoppingToken);

    public Task<Combo?> FirstOrDefaultAsync(Guid id, CancellationToken stoppingToken = default)
        => context.Combos.FirstOrDefaultAsync(c => c.Id == id, stoppingToken);

    public Task<ComboResponse> GetComboByIdForCreateResponse(Guid id, CancellationToken stoppingToken = default)
        => context.Combos
            .Include(c => c.Items)
            .ThenInclude(i => i.MenuItem)
            .Where(c => c.Id == id)
            .Select(c => new ComboResponse(
                c.Id,
                c.Name,
                c.Description,
                c.DiscountPercentage,
                c.Items.Select(i => new ComboItemResponse(i.MenuItem.Id, i.MenuItem.Name, i.MenuItem.Price)).ToList(),
                c.CreatedAt,
                c.UpdatedAt ?? c.CreatedAt))
            .FirstAsync(stoppingToken);

    public async Task<decimal> GetDiscountAsync(IEnumerable<Guid> menuItemIds, CancellationToken stoppingToken = default)
    {
        var orderItems = menuItemIds.ToHashSet();

        var combos = await context.Combos
            .AsNoTracking()
            .Select(c => new
            {
                c.DiscountPercentage,
                ItemIds = c.Items.Select(i => i.MenuItemId).ToList()
            })
            .ToListAsync(stoppingToken);

        return combos
            .Where(c => c.ItemIds.ToHashSet().SetEquals(orderItems))
            .Select(c => c.DiscountPercentage)
            .DefaultIfEmpty(0)
            .Max();
    }

    public void Add(Combo combo) => context.Combos.Add(combo);

    public void Remove(Combo combo) => context.Combos.Remove(combo);
}

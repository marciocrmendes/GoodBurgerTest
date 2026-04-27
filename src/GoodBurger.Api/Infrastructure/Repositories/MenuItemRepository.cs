using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Domain.Entities;
using GoodBurger.Api.Features.MenuItems._Shared;
using GoodBurger.Api.Infrastructure.Data;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoodBurger.Api.Infrastructure.Repositories;

public class MenuItemRepository(AppDbContext context) : IMenuItemRepository
{
    public Task<List<MenuItemResponse>> GetAllAsync(CancellationToken ct = default)
        => context.MenuItems
            .AsNoTracking()
            .Include(x => x.ItemCategory)
            .OrderBy(x => x.Name)
            .Select(x => new MenuItemResponse(x.Id, x.ItemCategoryId, x.ItemCategory.Name, x.Name, x.Price, x.Description))
            .ToListAsync(ct);

    public Task<MenuItemResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => context.MenuItems
            .AsNoTracking()
            .Include(x => x.ItemCategory)
            .Where(x => x.Id == id)
            .Select(x => new MenuItemResponse(x.Id, x.ItemCategoryId, x.ItemCategory.Name, x.Name, x.Price, x.Description))
            .FirstOrDefaultAsync(ct);

    public Task<MenuItem?> FindTrackedAsync(Guid id, CancellationToken ct = default)
        => context.MenuItems.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<MenuItemResponse> ReloadAsResponseAsync(Guid id, CancellationToken ct = default)
        => context.MenuItems
            .AsNoTracking()
            .Include(x => x.ItemCategory)
            .Where(x => x.Id == id)
            .Select(x => new MenuItemResponse(x.Id, x.ItemCategoryId, x.ItemCategory.Name, x.Name, x.Price, x.Description))
            .FirstAsync(ct);

    public Task<bool> CategoryExistsAsync(int categoryId, CancellationToken ct = default)
        => context.ItemCategories.AnyAsync(c => c.Id == categoryId, ct);

    public void Add(MenuItem item) => context.MenuItems.Add(item);

    public void Remove(MenuItem item) => context.MenuItems.Remove(item);

    public Task<List<MenuItemSnapshot>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        => context.MenuItems
            .AsNoTracking()
            .Include(x => x.ItemCategory)
            .Where(x => ids.Contains(x.Id))
            .Select(x => new MenuItemSnapshot(x.Id, x.Name, x.Price, x.ItemCategory.Name))
            .ToListAsync(ct);

    public Task<List<Guid>> GetExistingIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        => context.MenuItems
            .Where(x => ids.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync(ct);
}

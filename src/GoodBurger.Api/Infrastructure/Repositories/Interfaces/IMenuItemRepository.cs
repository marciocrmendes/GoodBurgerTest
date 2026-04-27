using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Domain.Entities;
using GoodBurger.Api.Features.MenuItems._Shared;

namespace GoodBurger.Api.Infrastructure.Repositories.Interfaces;

public interface IMenuItemRepository
{
    // CRUD
    Task<List<MenuItemResponse>> GetAllAsync(CancellationToken ct = default);
    Task<MenuItemResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<MenuItem?> FindTrackedAsync(Guid id, CancellationToken ct = default);
    Task<MenuItemResponse> ReloadAsResponseAsync(Guid id, CancellationToken ct = default);
    Task<bool> CategoryExistsAsync(int categoryId, CancellationToken ct = default);
    void Add(MenuItem item);
    void Remove(MenuItem item);

    // Used by order/combo handlers
    Task<List<MenuItemSnapshot>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    Task<List<Guid>> GetExistingIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}

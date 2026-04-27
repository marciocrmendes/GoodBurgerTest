using GoodBurger.Api.Domain.Entities;
using GoodBurger.Api.Features.Combos._Shared;

namespace GoodBurger.Api.Infrastructure.Repositories.Interfaces;

public interface IComboRepository
{
    Task<List<ComboResponse>> GetAllAsync(CancellationToken stoppingToken = default);
    Task<Combo?> FirstOrDefaultAsync(Guid id, CancellationToken stoppingToken = default);
    Task<ComboResponse> GetComboByIdForCreateResponse(Guid id, CancellationToken stoppingToken = default);
    Task<decimal> GetDiscountAsync(IEnumerable<Guid> menuItemIds, CancellationToken stoppingToken = default);
    void Add(Combo combo);
    void Remove(Combo combo);
}

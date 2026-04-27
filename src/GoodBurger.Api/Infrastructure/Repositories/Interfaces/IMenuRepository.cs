using GoodBurger.Api.Features.Menu;

namespace GoodBurger.Api.Infrastructure.Repositories.Interfaces;

public interface IMenuRepository
{
    Task<MenuResponse> GetMenuAsync(CancellationToken stoppingToken = default);
}

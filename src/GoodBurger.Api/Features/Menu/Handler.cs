using GoodBurger.Api.Domain.Abstractions;
using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;

namespace GoodBurger.Api.Features.Menu;

public class GetMenuHandler(IMenuRepository menuRepository) : IDomainHandler<GetMenuRequest, MenuResponse>
{
    public async Task<Result<MenuResponse>> HandleAsync(GetMenuRequest request, CancellationToken stoppingToken = default)
        => Result.Success(await menuRepository.GetMenuAsync(stoppingToken));
}

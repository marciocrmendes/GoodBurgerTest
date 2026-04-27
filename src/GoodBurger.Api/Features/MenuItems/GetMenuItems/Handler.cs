using GoodBurger.Api.Domain.Abstractions;
using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Features.MenuItems._Shared;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;

namespace GoodBurger.Api.Features.MenuItems.GetMenuItems;

public class GetMenuItemsHandler(IMenuItemRepository menuItemRepository) : IDomainHandler<GetMenuItemsRequest, List<MenuItemResponse>>
{
    public async Task<Result<List<MenuItemResponse>>> HandleAsync(GetMenuItemsRequest request, CancellationToken stoppingToken = default)
        => Result.Success(await menuItemRepository.GetAllAsync(stoppingToken));
}

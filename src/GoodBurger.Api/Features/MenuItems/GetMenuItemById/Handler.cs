using GoodBurger.Api.Domain.Abstractions;
using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Features.MenuItems._Shared;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;

namespace GoodBurger.Api.Features.MenuItems.GetMenuItemById;

public class GetMenuItemByIdHandler(IMenuItemRepository menuItemRepository) : IDomainHandler<GetMenuItemByIdRequest, MenuItemResponse>
{
    public async Task<Result<MenuItemResponse>> HandleAsync(GetMenuItemByIdRequest request, CancellationToken stoppingToken = default)
    {
        var item = await menuItemRepository.GetByIdAsync(request.Id, stoppingToken);
        return item is null
            ? Result.Failure<MenuItemResponse>(Error.NotFound("Item não encontrado."))
            : Result.Success(item);
    }
}

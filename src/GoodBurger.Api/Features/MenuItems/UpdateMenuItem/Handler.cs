using GoodBurger.Api.Domain.Abstractions;
using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Features.MenuItems._Shared;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;

namespace GoodBurger.Api.Features.MenuItems.UpdateMenuItem;

public class UpdateMenuItemHandler(
    IMenuItemRepository menuItemRepository,
    IUnitOfWork unitOfWork) : IDomainHandler<UpdateMenuItemRequest, MenuItemResponse>
{
    public async Task<Result<MenuItemResponse>> HandleAsync(UpdateMenuItemRequest request, CancellationToken stoppingToken = default)
    {
        var item = await menuItemRepository.FindTrackedAsync(request.Id, stoppingToken);
        if (item is null)
            return Result.Failure<MenuItemResponse>(Error.NotFound("Item não encontrado."));

        if (!await menuItemRepository.CategoryExistsAsync(request.CategoryId, stoppingToken))
            return Result.Failure<MenuItemResponse>(Error.NotFound("Categoria não encontrada."));

        item.Update(request.CategoryId, request.Name, request.Price, request.Description);
        await unitOfWork.CommitAsync(stoppingToken);

        return Result.Success(await menuItemRepository.ReloadAsResponseAsync(request.Id, stoppingToken));
    }
}

using GoodBurger.Api.Domain.Abstractions;
using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Domain.Entities;
using GoodBurger.Api.Features.MenuItems._Shared;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;

namespace GoodBurger.Api.Features.MenuItems.CreateMenuItem;

public class CreateMenuItemHandler(
    IMenuItemRepository menuItemRepository,
    IUnitOfWork unitOfWork) : IDomainHandler<CreateMenuItemRequest, MenuItemResponse>
{
    public async Task<Result<MenuItemResponse>> HandleAsync(CreateMenuItemRequest request, CancellationToken stoppingToken = default)
    {
        if (!await menuItemRepository.CategoryExistsAsync(request.CategoryId, stoppingToken))
            return Result.Failure<MenuItemResponse>(Error.NotFound("Categoria não encontrada."));

        var item = MenuItem.Create(request.CategoryId, request.Name, request.Price, request.Description ?? string.Empty);
        menuItemRepository.Add(item);
        await unitOfWork.CommitAsync(stoppingToken);

        return Result.Success(await menuItemRepository.ReloadAsResponseAsync(item.Id, stoppingToken));
    }
}

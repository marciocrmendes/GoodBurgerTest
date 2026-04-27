using GoodBurger.Api.Domain.Abstractions;
using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Domain.Entities;
using GoodBurger.Api.Features.Combos._Shared;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;

namespace GoodBurger.Api.Features.Combos.CreateCombo;

public class CreateComboHandler(
    IMenuItemRepository menuItemRepository,
    IComboRepository comboRepository,
    IUnitOfWork unitOfWork) : IDomainHandler<CreateComboRequest, ComboResponse>
{
    public async Task<Result<ComboResponse>> HandleAsync(CreateComboRequest request, CancellationToken stoppingToken = default)
    {
        var existingIds = await menuItemRepository.GetExistingIdsAsync(request.MenuItemIds, stoppingToken);
        var invalidItems = request.MenuItemIds.Except(existingIds).ToList();
        if (invalidItems.Count != 0)
            return Result.Failure<ComboResponse>(
                Error.Validation($"Itens inválidos: {string.Join(", ", invalidItems)}"));

        var combo = Combo.Create(request.Name, request.DiscountPercentage, request.Description, request.MenuItemIds);
        comboRepository.Add(combo);
        await unitOfWork.CommitAsync(stoppingToken);

        return Result.Success(await comboRepository.GetComboByIdForCreateResponse(combo.Id, stoppingToken));
    }
}

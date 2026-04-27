using GoodBurger.Api.Domain.Abstractions;
using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Features.Orders._Shared;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;

namespace GoodBurger.Api.Features.Orders.UpdateOrder;

public class UpdateOrderHandler(
    IOrderRepository orderRepository,
    IMenuItemRepository menuItemRepository,
    IComboRepository comboRepository,
    IUnitOfWork unitOfWork) : IDomainHandler<UpdateOrderRequest, OrderResponse>
{
    public async Task<Result<OrderResponse>> HandleAsync(UpdateOrderRequest request, CancellationToken stoppingToken = default)
    {
        var order = await orderRepository.FindTrackedWithItemsAsync(request.Id, stoppingToken);
        if (order is null)
            return Result.Failure<OrderResponse>(Error.NotFound("Pedido não encontrado."));

        var items = await menuItemRepository.GetByIdsAsync(request.MenuItemIds, stoppingToken);

        var missing = request.MenuItemIds.Except(items.Select(i => i.Id)).ToList();
        if (missing.Count != 0)
            return Result.Failure<OrderResponse>(Error.NotFound($"Itens não encontrados: {string.Join(", ", missing)}"));

        var categoryError = ValidateCategories(items);
        if (categoryError is not null)
            return Result.Failure<OrderResponse>(categoryError);

        var orderedItems = request.MenuItemIds.Select(id => items.First(i => i.Id == id)).ToList();
        var discountPercentage = await comboRepository.GetDiscountAsync(request.MenuItemIds, stoppingToken);

        var updateResult = order.Update(orderedItems, discountPercentage);
        if (updateResult.IsFailure)
            return Result.Failure<OrderResponse>(updateResult.Error);

        await unitOfWork.CommitAsync(stoppingToken);
        return Result.Success(await orderRepository.ReloadAsResponseAsync(request.Id, stoppingToken));
    }

    private static Error? ValidateCategories(List<MenuItemSnapshot> items)
    {
        var byCategory = items.GroupBy(i => i.CategoryName).ToDictionary(g => g.Key, g => g.Count());

        if (byCategory.GetValueOrDefault("Sanduíche") > 1)
            return Error.Validation("O pedido pode conter apenas um sanduíche.");
        if (byCategory.GetValueOrDefault("Batata") > 1)
            return Error.Validation("O pedido pode conter apenas uma batata.");
        return byCategory.GetValueOrDefault("Bebida") > 1 ? Error.Validation("O pedido pode conter apenas uma bebida.") : null;
    }
}

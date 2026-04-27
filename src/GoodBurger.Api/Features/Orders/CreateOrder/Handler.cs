using GoodBurger.Api.Domain.Abstractions;
using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Domain.Entities;
using GoodBurger.Api.Features.Orders._Shared;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;

namespace GoodBurger.Api.Features.Orders.CreateOrder;

public class CreateOrderHandler(
    IMenuItemRepository menuItemRepository,
    IComboRepository comboRepository,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : IDomainHandler<CreateOrderRequest, OrderResponse>
{
    public async Task<Result<OrderResponse>> HandleAsync(CreateOrderRequest request, CancellationToken stoppingToken = default)
    {
        var items = await menuItemRepository.GetByIdsAsync(request.MenuItemIds, stoppingToken);

        var missing = request.MenuItemIds.Except(items.Select(x => x.Id)).ToList();
        if (missing.Count != 0)
            return Result.Failure<OrderResponse>(Error.NotFound($"Itens não encontrados: {string.Join(", ", missing)}"));

        var categoryError = ValidateCategories(items);
        if (categoryError is not null)
            return Result.Failure<OrderResponse>(categoryError);

        var orderedItems = request.MenuItemIds.Select(id => items.First(x => x.Id == id)).ToList();
        var discountPercentage = await comboRepository.GetDiscountAsync(request.MenuItemIds, stoppingToken);

        var orderResult = Order.Create(orderedItems, discountPercentage);
        if (orderResult.IsFailure)
            return Result.Failure<OrderResponse>(orderResult.Error);

        orderRepository.Add(orderResult.Value);
        await unitOfWork.CommitAsync(stoppingToken);

        return Result.Success(await orderRepository.ReloadAsResponseAsync(orderResult.Value.Id, stoppingToken));
    }

    private static Error? ValidateCategories(List<MenuItemSnapshot> items)
    {
        var byCategory = items.GroupBy(i => i.CategoryName).ToDictionary(g => g.Key, g => g.Count());

        foreach (var category in byCategory.Where(c => c.Value > 1))
            return Error.Validation($"Apenas um item da categoria '{category.Key}' pode ser adicionado por pedido.");

        return null;
    }
}

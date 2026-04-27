using GoodBurger.Api.Domain.Entities;

namespace GoodBurger.Api.Domain.Services;

public class DiscountCalculator
{
    public decimal Calculate(IEnumerable<Combo> combos, IEnumerable<Guid> orderItemIds)
    {
        var orderItems = orderItemIds.ToHashSet();

        return combos
            .Where(c =>
            {
                var comboItems = c.Items.Select(i => i.MenuItemId).ToHashSet();
                return comboItems.SetEquals(orderItems);
            })
            .Select(c => c.DiscountPercentage)
            .DefaultIfEmpty(0)
            .Max();
    }
}

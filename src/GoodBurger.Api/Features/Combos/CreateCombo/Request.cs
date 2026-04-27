namespace GoodBurger.Api.Features.Combos.CreateCombo;

public record CreateComboRequest(
    string Name,
    string Description,
    decimal DiscountPercentage,
    List<Guid> MenuItemIds);

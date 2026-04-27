namespace GoodBurger.Api.Features.Combos._Shared;

public record ComboItemResponse(Guid MenuItemId, string MenuItemName, decimal Price);

public record ComboResponse(
    Guid Id,
    string Name,
    string Description,
    decimal DiscountPercentage,
    List<ComboItemResponse> Items,
    DateTime CreatedAt,
    DateTime UpdatedAt);

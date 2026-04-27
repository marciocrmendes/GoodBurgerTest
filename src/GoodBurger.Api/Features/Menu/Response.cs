namespace GoodBurger.Api.Features.Menu;

public record MenuItemDto(
    Guid Id,
    string Name,
    string Category,
    decimal Price);

public record ComboDto(
    Guid Id,
    string Name,
    string Description,
    decimal DiscountPercentage,
    List<MenuItemDto> Items);

public record MenuResponse(
    List<MenuItemDto> Items,
    List<ComboDto> Combos);

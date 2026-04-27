namespace GoodBurger.Api.Features.MenuItems._Shared;

public record MenuItemResponse(
    Guid Id,
    int CategoryId,
    string Category,
    string Name,
    decimal Price,
    string? Description);

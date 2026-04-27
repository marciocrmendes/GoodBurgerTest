namespace GoodBurger.Api.Features.MenuItems.UpdateMenuItem;

public record UpdateMenuItemRequest(
    Guid Id,
    int CategoryId,
    string Name,
    decimal Price,
    string? Description);

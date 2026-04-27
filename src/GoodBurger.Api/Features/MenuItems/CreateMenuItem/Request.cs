namespace GoodBurger.Api.Features.MenuItems.CreateMenuItem;

public record CreateMenuItemRequest(
    int CategoryId,
    string Name,
    decimal Price,
    string? Description);

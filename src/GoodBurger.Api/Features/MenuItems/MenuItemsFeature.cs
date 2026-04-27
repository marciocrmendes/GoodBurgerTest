using GoodBurger.Api.Features.MenuItems.CreateMenuItem;
using GoodBurger.Api.Features.MenuItems.DeleteMenuItem;
using GoodBurger.Api.Features.MenuItems.GetMenuItemById;
using GoodBurger.Api.Features.MenuItems.GetMenuItems;
using GoodBurger.Api.Features.MenuItems.UpdateMenuItem;

namespace GoodBurger.Api.Features.MenuItems;

public static class MenuItemsFeature
{
    public static void MapMenuItemsFeature(this WebApplication app)
    {
        var group = app.MapGroup("/menu-items")
            .WithTags("Cardápio")
            .RequireAuthorization();

        group.MapGetMenuItemsEndpoint();
        group.MapGetMenuItemByIdEndpoint();
        group.MapCreateMenuItemEndpoint();
        group.MapUpdateMenuItemEndpoint();
        group.MapDeleteMenuItemEndpoint();
    }
}

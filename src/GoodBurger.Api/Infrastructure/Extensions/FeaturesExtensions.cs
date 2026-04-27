using GoodBurger.Api.Features.Auth;
using GoodBurger.Api.Features.Combos;
using GoodBurger.Api.Features.Menu;
using GoodBurger.Api.Features.MenuItems;
using GoodBurger.Api.Features.Orders;

namespace GoodBurger.Api.Infrastructure.Extensions;

public static class FeaturesExtensions
{
    public static WebApplication MapFeatureEndpoints(this WebApplication app)
    {
        app.MapAuthEndpoints();
        app.MapMenuEndpoints();
        app.MapMenuItemsFeature();
        app.MapOrdersFeature();
        app.MapCombosFeature();
        return app;
    }
}

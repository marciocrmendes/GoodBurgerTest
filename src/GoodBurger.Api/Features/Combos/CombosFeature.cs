using GoodBurger.Api.Features.Combos.CreateCombo;
using GoodBurger.Api.Features.Combos.DeleteCombo;
using GoodBurger.Api.Features.Combos.GetCombos;

namespace GoodBurger.Api.Features.Combos;

public static class CombosFeature
{
    public static void MapCombosFeature(this WebApplication app)
    {
        var group = app.MapGroup("/combos")
            .WithTags("Combos")
            .RequireAuthorization();

        group.MapGetCombosEndpoint();
        group.MapCreateComboEndpoint();
        group.MapDeleteComboEndpoint();
    }
}

using GoodBurger.Api.Features.Menu;
using GoodBurger.Api.Infrastructure.Data;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoodBurger.Api.Infrastructure.Repositories;

public class MenuRepository(AppDbContext context) : IMenuRepository
{
    public async Task<MenuResponse> GetMenuAsync(CancellationToken stoppingToken = default)
    {
        var items = await context.MenuItems
            .AsNoTracking()
            .Include(x => x.ItemCategory)
            .OrderBy(x => x.Name)
            .Select(x => new MenuItemDto(x.Id, x.Name, x.ItemCategory.Name, x.Price))
            .ToListAsync(stoppingToken);

        var combos = await context.Combos
            .AsNoTracking()
            .Include(x => x.Items)
            .ThenInclude(x => x.MenuItem)
            .ThenInclude(x => x.ItemCategory)
            .OrderBy(x => x.Name)
            .Select(x => new ComboDto(
                x.Id,
                x.Name,
                x.Description,
                x.DiscountPercentage,
                x.Items.Select(ci => new MenuItemDto(
                    ci.MenuItem.Id,
                    ci.MenuItem.Name,
                    ci.MenuItem.ItemCategory.Name,
                    ci.MenuItem.Price)).ToList()))
            .ToListAsync(stoppingToken);

        return new MenuResponse(items, combos);
    }
}

using System.Net.Http.Json;
using GoodBurger.Web.Models;

namespace GoodBurger.Web.Services;

public interface IMenuService
{
    Task<List<MenuItemModel>> GetMenuAsync();
}

public class MenuService(HttpClient httpClient) : IMenuService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<List<MenuItemModel>> GetMenuAsync()
    {
        var items = await _httpClient.GetFromJsonAsync<List<MenuItemModel>>("menu");
        return items ?? new List<MenuItemModel>();
    }
}

using System.Net.Http.Json;
using GoodBurger.Web.Models;

namespace GoodBurger.Web.Services;

public interface IOrderService
{
    Task<List<OrderResponse>> GetOrdersAsync();
    Task<OrderResponse?> GetOrderByIdAsync(Guid id);
    Task<(OrderResponse? Order, string? Error)> CreateOrderAsync(CreateOrderRequest request);
    Task<(OrderResponse? Order, string? Error)> UpdateOrderAsync(Guid id, UpdateOrderRequest request);
    Task<(bool Success, string? Error)> DeleteOrderAsync(Guid id);
}

public class OrderService(HttpClient httpClient) : IOrderService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<List<OrderResponse>> GetOrdersAsync()
    {
        var orders = await _httpClient.GetFromJsonAsync<List<OrderResponse>>("orders");
        return orders ?? new List<OrderResponse>();
    }

    public async Task<OrderResponse?> GetOrderByIdAsync(Guid id)
        => await _httpClient.GetFromJsonAsync<OrderResponse>($"orders/{id}");

    public async Task<(OrderResponse? Order, string? Error)> CreateOrderAsync(CreateOrderRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("orders", request);
        if (response.IsSuccessStatusCode)
        {
            var order = await response.Content.ReadFromJsonAsync<OrderResponse>();
            return (order, null);
        }
        var error = await response.Content.ReadFromJsonAsync<ApiError>();
        return (null, error?.Message ?? "Erro ao criar pedido.");
    }

    public async Task<(OrderResponse? Order, string? Error)> UpdateOrderAsync(Guid id, UpdateOrderRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"orders/{id}", request);
        if (response.IsSuccessStatusCode)
        {
            var order = await response.Content.ReadFromJsonAsync<OrderResponse>();
            return (order, null);
        }
        var error = await response.Content.ReadFromJsonAsync<ApiError>();
        return (null, error?.Message ?? "Erro ao atualizar pedido.");
    }

    public async Task<(bool Success, string? Error)> DeleteOrderAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"orders/{id}");
        if (response.IsSuccessStatusCode)
            return (true, null);
        var error = await response.Content.ReadFromJsonAsync<ApiError>();
        return (false, error?.Message ?? "Erro ao remover pedido.");
    }
}

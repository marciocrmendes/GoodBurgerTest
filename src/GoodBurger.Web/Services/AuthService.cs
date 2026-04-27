using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using GoodBurger.Web.Models;

namespace GoodBurger.Web.Services;

public class AuthService(
    HttpClient httpClient,
    ILocalStorageService localStorage,
    AuthenticationStateProvider authStateProvider)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILocalStorageService _localStorage = localStorage;
    private readonly AuthenticationStateProvider _authStateProvider = authStateProvider;
    private const string TokenKey = "authToken";

    public async Task<(bool Success, string? ErrorMessage)> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ApiError>();
                return (false, error?.Message ?? "Credenciais inválidas.");
            }

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (loginResponse is null)
                return (false, "Resposta inválida do servidor.");

            await _localStorage.SetItemAsync(TokenKey, loginResponse.Token);
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResponse.Token);

            ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(loginResponse.Token);
            return (true, null);
        }
        catch
        {
            return (false, "Erro ao conectar com o servidor.");
        }
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(TokenKey);
        _httpClient.DefaultRequestHeaders.Authorization = null;
        ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
    }

    public async Task<string?> GetTokenAsync()
        => await _localStorage.GetItemAsync<string>(TokenKey);
}

using Blazored.LocalStorage;
using GoodBurger.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<GoodBurger.Web.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// API base URL - will be configured via appsettings
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:8080/";
if (!apiBaseUrl.EndsWith('/'))
    apiBaseUrl += '/';

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

// MudBlazor
builder.Services.AddMudServices();

// Local Storage
builder.Services.AddBlazoredLocalStorage();

// Auth
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<AuthService>();

// App Services
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IOrderService, OrderService>();

await builder.Build().RunAsync();

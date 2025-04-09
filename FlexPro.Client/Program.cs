using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FlexPro.Client;
using MudBlazor.Services;
using FlexPro.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using FlexPro.Client.Providers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    var client = new HttpClient
    {
        BaseAddress = new Uri("https://flexpro-api.onrender.com")
    };
    return client;
});
builder.Services.AddMudServices();

// Registros de Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<LocalStorageService>();

// autentica��o
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthorizationService, DefaultAuthorizationService>();



await builder.Build().RunAsync();

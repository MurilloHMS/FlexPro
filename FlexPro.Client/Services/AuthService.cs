using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FlexPro.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace FlexPro.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly NavigationManager _navigationManager;
        private readonly LocalStorageService _localStorageService;

        public AuthService(HttpClient httpClient, IConfiguration configuration, NavigationManager navigationManager, LocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
        }

        public async Task<string> Login(LoginModel loginModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);

            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
                if(result?.Token != null)
                {
                    await _localStorageService.SetItemAsync("authToken", result.Token);
                    return result.Token;
                }
            }

            return null;
        }

        public async Task<bool> Register(RegisterModel registerModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerModel);
            return response.IsSuccessStatusCode;
        }

        public async Task<string> GetAuthToken()
        {
            return await _localStorageService.GetItemAsync("authToken");
        }

        public async Task Logout()
        {
            await _localStorageService.RemoveItemAsync("authToken");
            _navigationManager.NavigateTo("/Account/Login");
        }
    }
}

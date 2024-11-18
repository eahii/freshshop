using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using UsedPhonesWebShop.Shared.DTOs;

namespace UsedPhonesWebShop.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;

        public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> Register(UserRegistrationDto registrationDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/register", registrationDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<string?> Login(UserLoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", loginDto);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResult>();
                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "jwtToken", result.Token);
                    return result.Token;
                }
            }
            return null;
        }

        public async Task Logout()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "jwtToken");
        }

        private class LoginResult
        {
            public string Token { get; set; } = string.Empty;
        }
    }
}
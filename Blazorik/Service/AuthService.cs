using Blazored.LocalStorage;
using Blazorik.Shared;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    // Метод логина
    public async Task<LoginResponse> LoginAsync(string email, string password)
    {
        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = password
        };

        var response = await _httpClient.PostAsJsonAsync("https://localhost:7231/api/Auth/Login", loginRequest);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException($"Login failed with status code {response.StatusCode}");
        }

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        await SetTokensAsync(result);

        return result;
    }

    private async Task SetTokensAsync(LoginResponse loginResponse)
    {
        if (loginResponse != null)
        {
            await _localStorage.SetItemAsync("accessToken", loginResponse.AccessToken);
            await _localStorage.SetItemAsync("refreshToken", loginResponse.RefreshToken);
        }
    }
}




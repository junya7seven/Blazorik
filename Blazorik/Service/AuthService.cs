using Blazored.LocalStorage;
using Blazorik.Shared;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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
        // Создаем объект LoginRequest
        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = password
        };

        // Отправляем POST-запрос
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7231/api/Auth/Login", loginRequest);

        // Проверяем, был ли запрос успешным
        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException($"Login failed with status code {response.StatusCode}");
        }

        // Читаем и возвращаем результат (например, токены)
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        // Сохраняем токены в локальном хранилище
        await SetTokensAsync(result);

        return result;
    }

    // Метод для сохранения токенов в локальном хранилище
    private async Task SetTokensAsync(LoginResponse loginResponse)
    {
        if (loginResponse != null)
        {
            await _localStorage.SetItemAsync("accessToken", loginResponse.AccessToken);
            await _localStorage.SetItemAsync("refreshToken", loginResponse.RefreshToken);
        }
    }

    // Метод для получения access токена
    public async Task<string> GetAccessTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>("accessToken");
    }

    // Метод для выхода
    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("accessToken");
        await _localStorage.RemoveItemAsync("refreshToken");
    }
}




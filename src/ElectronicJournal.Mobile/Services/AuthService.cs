using System.Net.Http.Json;
using ElectronicJournal.Mobile.Models;

namespace ElectronicJournal.Mobile.Services;

public interface IAuthService
{
    Task<AuthResponse?> LoginAsync(string email, string password);
    Task LogoutAsync();
    Task<string?> GetTokenAsync();
    Task<User?> GetCurrentUserAsync();
    Task<bool> IsAuthenticatedAsync();
}

public class AuthService : IAuthService
{
    private const string TokenKey = "auth_token";
    private const string RefreshTokenKey = "refresh_token";
    private const string UserIdKey = "user_id";
    private const string UserRoleKey = "user_role";

    private readonly HttpClient _httpClient;
    private User? _currentUser;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AuthResponse?> LoginAsync(string email, string password)
    {
        try
        {
            var request = new LoginRequest { Email = email, Password = password };
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

            if (!response.IsSuccessStatusCode)
                return null;

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if (authResponse == null) return null;

            await SecureStorage.SetAsync(TokenKey, authResponse.Token);
            await SecureStorage.SetAsync(RefreshTokenKey, authResponse.RefreshToken);
            await SecureStorage.SetAsync(UserIdKey, authResponse.User.Id.ToString());
            await SecureStorage.SetAsync(UserRoleKey, authResponse.User.Role);

            _currentUser = authResponse.User;
            return authResponse;
        }
        catch
        {
            return null;
        }
    }

    public async Task LogoutAsync()
    {
        SecureStorage.Remove(TokenKey);
        SecureStorage.Remove(RefreshTokenKey);
        SecureStorage.Remove(UserIdKey);
        SecureStorage.Remove(UserRoleKey);
        _currentUser = null;
        await Task.CompletedTask;
    }

    public async Task<string?> GetTokenAsync()
    {
        return await SecureStorage.GetAsync(TokenKey);
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        if (_currentUser != null) return _currentUser;

        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token)) return null;

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            _currentUser = await _httpClient.GetFromJsonAsync<User>("api/auth/me");
            return _currentUser;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrEmpty(token);
    }
}

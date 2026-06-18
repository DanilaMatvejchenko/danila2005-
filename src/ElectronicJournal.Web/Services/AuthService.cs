using System.Net.Http.Json;
using Blazored.LocalStorage;
using ElectronicJournal.Web.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace ElectronicJournal.Web.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;

    private const string TokenKey = "authToken";
    private const string UserKey = "authUser";

    public AuthService(HttpClient http, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
    {
        _http = http;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", request);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (result is null) return null;

        await _localStorage.SetItemAsStringAsync(TokenKey, result.Token);
        await _localStorage.SetItemAsync(UserKey, result);

        ((CustomAuthStateProvider)_authStateProvider).NotifyAuthenticationStateChanged();
        return result;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(TokenKey);
        await _localStorage.RemoveItemAsync(UserKey);
        ((CustomAuthStateProvider)_authStateProvider).NotifyAuthenticationStateChanged();
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _localStorage.GetItemAsStringAsync(TokenKey);
    }
}

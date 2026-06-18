using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ElectronicJournal.Web.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _http;
    private readonly IAuthService _authService;

    public ApiService(HttpClient http, IAuthService authService)
    {
        _http = http;
        _authService = authService;
    }

    private async Task SetAuthHeader()
    {
        var token = await _authService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        await SetAuthHeader();
        try
        {
            return await _http.GetFromJsonAsync<T>(url);
        }
        catch
        {
            return default;
        }
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
    {
        await SetAuthHeader();
        try
        {
            var response = await _http.PostAsJsonAsync(url, data);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<TResponse>();
            return default;
        }
        catch
        {
            return default;
        }
    }

    public async Task<bool> PostAsync<TRequest>(string url, TRequest data)
    {
        await SetAuthHeader();
        try
        {
            var response = await _http.PostAsJsonAsync(url, data);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> PutAsync<TRequest>(string url, TRequest data)
    {
        await SetAuthHeader();
        try
        {
            var response = await _http.PutAsJsonAsync(url, data);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(string url)
    {
        await SetAuthHeader();
        try
        {
            var response = await _http.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}

using Blazored.LocalStorage;

namespace ElectronicJournal.Web.Services;

public class ThemeService
{
    private readonly ILocalStorageService _localStorage;
    private const string ThemeKey = "darkMode";

    public ThemeService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<bool> IsDarkMode()
    {
        return await _localStorage.GetItemAsync<bool>(ThemeKey);
    }

    public async Task SetDarkMode(bool isDark)
    {
        await _localStorage.SetItemAsync(ThemeKey, isDark);
    }
}

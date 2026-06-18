using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicJournal.Mobile.Services;

namespace ElectronicJournal.Mobile.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private bool _hasError;

    public LoginViewModel(IAuthService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Заполните все поля";
            HasError = true;
            return;
        }

        IsBusy = true;
        HasError = false;

        try
        {
            var result = await _authService.LoginAsync(Email, Password);
            if (result != null)
            {
                await _navigationService.NavigateToAsync("//dashboard");
            }
            else
            {
                ErrorMessage = "Неверный email или пароль";
                HasError = true;
            }
        }
        catch (Exception)
        {
            ErrorMessage = "Ошибка подключения к серверу";
            HasError = true;
        }
        finally
        {
            IsBusy = false;
        }
    }
}

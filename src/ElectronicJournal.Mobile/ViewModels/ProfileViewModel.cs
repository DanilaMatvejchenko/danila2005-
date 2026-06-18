using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicJournal.Mobile.Models;
using ElectronicJournal.Mobile.Services;

namespace ElectronicJournal.Mobile.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly IApiService _apiService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _fullName = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _role = string.Empty;

    [ObservableProperty]
    private string? _phone;

    [ObservableProperty]
    private string? _groupName;

    [ObservableProperty]
    private string _initials = string.Empty;

    [ObservableProperty]
    private bool _isEditing;

    private User? _currentUser;

    public ProfileViewModel(IAuthService authService, IApiService apiService, INavigationService navigationService)
    {
        _authService = authService;
        _apiService = apiService;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task LoadProfileAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            _currentUser = await _authService.GetCurrentUserAsync();
            if (_currentUser != null)
            {
                FullName = _currentUser.FullName;
                Email = _currentUser.Email;
                Role = _currentUser.Role switch
                {
                    "Student" => "Студент",
                    "Teacher" => "Преподаватель",
                    "Admin" => "Администратор",
                    _ => _currentUser.Role
                };
                Phone = _currentUser.Phone;
                GroupName = _currentUser.GroupName;
                Initials = $"{(_currentUser.FirstName.Length > 0 ? _currentUser.FirstName[0] : ' ')}{(_currentUser.LastName.Length > 0 ? _currentUser.LastName[0] : ' ')}";
            }
        }
        catch (Exception)
        {
            // Handle error
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void ToggleEdit()
    {
        IsEditing = !IsEditing;
    }

    [RelayCommand]
    private async Task SaveProfileAsync()
    {
        if (_currentUser == null) return;
        IsBusy = true;

        try
        {
            _currentUser.Phone = Phone;
            var updated = await _apiService.UpdateProfileAsync(_currentUser);
            if (updated != null)
            {
                _currentUser = updated;
                IsEditing = false;
            }
        }
        catch (Exception)
        {
            // Handle error
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _authService.LogoutAsync();
        await _navigationService.NavigateToAsync("//login");
    }
}

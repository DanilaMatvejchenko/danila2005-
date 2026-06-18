using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicJournal.Mobile.Models;
using ElectronicJournal.Mobile.Services;

namespace ElectronicJournal.Mobile.ViewModels;

public partial class AttendanceViewModel : ObservableObject
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private bool _isRefreshing;

    [ObservableProperty]
    private double _attendancePercent;

    public ObservableCollection<Attendance> Records { get; } = new();

    public AttendanceViewModel(IApiService apiService)
    {
        _apiService = apiService;
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            var records = await _apiService.GetAttendanceAsync();
            Records.Clear();
            foreach (var record in records)
                Records.Add(record);

            if (records.Count > 0)
            {
                var present = records.Count(r => r.Status == AttendanceStatus.Present || r.Status == AttendanceStatus.Late);
                AttendancePercent = Math.Round((double)present / records.Count * 100, 1);
            }
        }
        catch (Exception)
        {
            // Handle error
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadDataAsync();
    }
}

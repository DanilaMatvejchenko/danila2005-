using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicJournal.Mobile.Models;
using ElectronicJournal.Mobile.Services;

namespace ElectronicJournal.Mobile.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private double _averageGrade;

    [ObservableProperty]
    private double _attendancePercent;

    [ObservableProperty]
    private int _todayLessonsCount;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private bool _isRefreshing;

    public ObservableCollection<Grade> RecentGrades { get; } = new();
    public ObservableCollection<Schedule> TodaySchedule { get; } = new();

    public DashboardViewModel(IApiService apiService)
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
            var stats = await _apiService.GetDashboardStatsAsync();
            AverageGrade = stats.AverageGrade;
            AttendancePercent = stats.AttendancePercent;
            TodayLessonsCount = stats.TodayLessonsCount;

            RecentGrades.Clear();
            foreach (var grade in stats.RecentGrades)
                RecentGrades.Add(grade);

            TodaySchedule.Clear();
            foreach (var lesson in stats.TodaySchedule)
                TodaySchedule.Add(lesson);
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

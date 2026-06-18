using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicJournal.Mobile.Models;
using ElectronicJournal.Mobile.Services;

namespace ElectronicJournal.Mobile.ViewModels;

public partial class JournalViewModel : ObservableObject
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private bool _isRefreshing;

    [ObservableProperty]
    private string? _selectedSubject;

    public ObservableCollection<GradeGroup> GradeGroups { get; } = new();
    public ObservableCollection<string> Subjects { get; } = new();

    public JournalViewModel(IApiService apiService)
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
            var groups = await _apiService.GetGradesGroupedAsync();

            Subjects.Clear();
            Subjects.Add("Все предметы");
            foreach (var g in groups)
                Subjects.Add(g.SubjectName);

            GradeGroups.Clear();
            var filtered = string.IsNullOrEmpty(SelectedSubject) || SelectedSubject == "Все предметы"
                ? groups
                : groups.Where(g => g.SubjectName == SelectedSubject).ToList();

            foreach (var group in filtered)
                GradeGroups.Add(group);
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
    private async Task FilterBySubjectAsync()
    {
        await LoadDataAsync();
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadDataAsync();
    }
}

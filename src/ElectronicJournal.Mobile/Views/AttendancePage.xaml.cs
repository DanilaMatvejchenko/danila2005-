using ElectronicJournal.Mobile.ViewModels;

namespace ElectronicJournal.Mobile.Views;

public partial class AttendancePage : ContentPage
{
    private readonly AttendanceViewModel _viewModel;

    public AttendancePage(AttendanceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadDataCommand.ExecuteAsync(null);
    }
}

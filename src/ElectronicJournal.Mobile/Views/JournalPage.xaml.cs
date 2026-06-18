using ElectronicJournal.Mobile.ViewModels;

namespace ElectronicJournal.Mobile.Views;

public partial class JournalPage : ContentPage
{
    private readonly JournalViewModel _viewModel;

    public JournalPage(JournalViewModel viewModel)
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

using ElectronicJournal.Mobile.ViewModels;

namespace ElectronicJournal.Mobile.Views;

public partial class ChatPage : ContentPage
{
    private readonly ChatViewModel _viewModel;

    public ChatPage(ChatViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadMessagesCommand.ExecuteAsync(null);
    }
}

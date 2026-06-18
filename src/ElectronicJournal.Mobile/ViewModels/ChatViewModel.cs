using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicJournal.Mobile.Models;
using ElectronicJournal.Mobile.Services;

namespace ElectronicJournal.Mobile.ViewModels;

public partial class ChatViewModel : ObservableObject
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _messageText = string.Empty;

    [ObservableProperty]
    private bool _isRefreshing;

    public ObservableCollection<ChatMessage> Messages { get; } = new();

    public ChatViewModel(IApiService apiService)
    {
        _apiService = apiService;
    }

    [RelayCommand]
    private async Task LoadMessagesAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            var messages = await _apiService.GetMessagesAsync();
            Messages.Clear();
            foreach (var msg in messages)
                Messages.Add(msg);
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
    private async Task SendMessageAsync()
    {
        if (string.IsNullOrWhiteSpace(MessageText)) return;

        var message = new ChatMessage
        {
            Text = MessageText,
            SentAt = DateTime.Now,
            IsMine = true
        };

        try
        {
            var sent = await _apiService.SendMessageAsync(message);
            if (sent != null)
            {
                Messages.Add(sent);
                MessageText = string.Empty;
            }
        }
        catch (Exception)
        {
            // Handle error
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadMessagesAsync();
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ElectronicJournal.API.Hubs;

/// <summary>
/// SignalR хаб для real-time уведомлений журнала.
/// </summary>
[Authorize]
public class JournalHub : Hub
{
    /// <summary>
    /// Отправить уведомление о новой оценке студенту.
    /// </summary>
    public async Task SendGradeNotification(int studentUserId, string subject, int value)
    {
        await Clients.User(studentUserId.ToString())
            .SendAsync("ReceiveGradeNotification", subject, value);
    }

    /// <summary>
    /// Отправить сообщение чата.
    /// </summary>
    public async Task SendChatMessage(int receiverUserId, string message)
    {
        await Clients.User(receiverUserId.ToString())
            .SendAsync("ReceiveChatMessage", Context.UserIdentifier, message);
    }

    /// <summary>
    /// Отправить обновление посещаемости.
    /// </summary>
    public async Task SendAttendanceUpdate(int studentUserId, string subject, string status)
    {
        await Clients.User(studentUserId.ToString())
            .SendAsync("ReceiveAttendanceUpdate", subject, status);
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (userId is not null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (userId is not null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
        }
        await base.OnDisconnectedAsync(exception);
    }
}

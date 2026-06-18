using ElectronicJournal.API.Models;

namespace ElectronicJournal.API.Services;

public interface IChatService
{
    Task<ChatMessage> SendMessageAsync(int senderId, int receiverId, string text);
    Task<IEnumerable<ChatMessage>> GetConversationAsync(int userId1, int userId2);
    Task MarkAsReadAsync(int messageId, int userId);
}

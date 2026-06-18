using ElectronicJournal.API.Data;
using ElectronicJournal.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.API.Services;

public class ChatService : IChatService
{
    private readonly ApplicationDbContext _db;

    public ChatService(ApplicationDbContext db) => _db = db;

    public async Task<ChatMessage> SendMessageAsync(int senderId, int receiverId, string text)
    {
        var message = new ChatMessage
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Text = text,
            SentAt = DateTime.UtcNow,
            IsRead = false
        };
        _db.ChatMessages.Add(message);
        await _db.SaveChangesAsync();
        return message;
    }

    public async Task<IEnumerable<ChatMessage>> GetConversationAsync(int userId1, int userId2) =>
        await _db.ChatMessages
            .Include(m => m.Sender).Include(m => m.Receiver)
            .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                        (m.SenderId == userId2 && m.ReceiverId == userId1))
            .OrderBy(m => m.SentAt)
            .ToListAsync();

    public async Task MarkAsReadAsync(int messageId, int userId)
    {
        var message = await _db.ChatMessages.FindAsync(messageId);
        if (message is not null && message.ReceiverId == userId)
        {
            message.IsRead = true;
            await _db.SaveChangesAsync();
        }
    }
}

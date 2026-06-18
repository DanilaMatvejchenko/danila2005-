namespace ElectronicJournal.Mobile.Models;

public class ChatMessage
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string? SenderAvatarUrl { get; set; }
    public int? RecipientId { get; set; }
    public int? GroupChatId { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
    public bool IsMine { get; set; }

    public string TimeText => SentAt.ToString("HH:mm");
    public string DateText => SentAt.Date == DateTime.Today
        ? "Сегодня"
        : SentAt.Date == DateTime.Today.AddDays(-1)
            ? "Вчера"
            : SentAt.ToString("dd.MM.yyyy");
}

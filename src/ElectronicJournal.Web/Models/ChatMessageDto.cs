namespace ElectronicJournal.Web.Models;

public class ChatMessageDto
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public string SenderName { get; set; } = "";
    public int ReceiverId { get; set; }
    public string ReceiverName { get; set; } = "";
    public string Text { get; set; } = "";
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
}

public class ChatContactDto
{
    public int UserId { get; set; }
    public string Name { get; set; } = "";
    public string LastMessage { get; set; } = "";
    public DateTime LastMessageTime { get; set; }
    public int UnreadCount { get; set; }
}

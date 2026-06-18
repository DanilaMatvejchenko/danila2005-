namespace ElectronicJournal.API.Models;

public class Teacher
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string Department { get; set; } = string.Empty;
}

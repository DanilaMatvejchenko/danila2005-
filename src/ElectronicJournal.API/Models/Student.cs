namespace ElectronicJournal.API.Models;

public class Student
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;
    public int? ParentId { get; set; }
    public User? Parent { get; set; }
}

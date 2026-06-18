namespace ElectronicJournal.API.Models;

public class TeacherSubject
{
    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; } = null!;
    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;
}

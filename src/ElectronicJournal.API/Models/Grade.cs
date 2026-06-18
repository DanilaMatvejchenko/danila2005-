namespace ElectronicJournal.API.Models;

public enum GradeType
{
    Current,
    Midterm,
    Final
}

public class Grade
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; } = null!;
    public int Value { get; set; }
    public DateTime Date { get; set; }
    public string? Comment { get; set; }
    public GradeType GradeType { get; set; }
}

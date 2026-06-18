using ElectronicJournal.API.Models;

namespace ElectronicJournal.API.DTOs;

public class CreateGradeDto
{
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public int TeacherId { get; set; }
    public int Value { get; set; }
    public DateTime Date { get; set; }
    public string? Comment { get; set; }
    public GradeType GradeType { get; set; }
}

public class GradeResponseDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public int TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public int Value { get; set; }
    public DateTime Date { get; set; }
    public string? Comment { get; set; }
    public string GradeType { get; set; } = string.Empty;
}

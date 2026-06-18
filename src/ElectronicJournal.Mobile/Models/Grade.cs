namespace ElectronicJournal.Mobile.Models;

public class Grade
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public int Value { get; set; }
    public DateTime Date { get; set; }
    public string? Comment { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public string GradeType { get; set; } = string.Empty; // Текущая, Контрольная, Экзамен

    public Color GradeColor => Value switch
    {
        5 => Color.FromArgb("#16a34a"),
        4 => Color.FromArgb("#2563eb"),
        3 => Color.FromArgb("#f59e0b"),
        2 => Color.FromArgb("#dc2626"),
        _ => Color.FromArgb("#6b7280")
    };
}

public class GradeGroup
{
    public string SubjectName { get; set; } = string.Empty;
    public List<Grade> Grades { get; set; } = new();
    public double Average => Grades.Count > 0 ? Math.Round(Grades.Average(g => g.Value), 2) : 0;
}

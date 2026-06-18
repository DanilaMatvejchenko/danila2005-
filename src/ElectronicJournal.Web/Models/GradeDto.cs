namespace ElectronicJournal.Web.Models;

public class GradeDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = "";
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = "";
    public int Value { get; set; }
    public DateTime Date { get; set; }
    public string Comment { get; set; } = "";
    public string GradeType { get; set; } = "";
}

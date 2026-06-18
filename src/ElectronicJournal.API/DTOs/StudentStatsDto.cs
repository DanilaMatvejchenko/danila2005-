namespace ElectronicJournal.API.DTOs;

public class StudentStatsDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public double AverageGrade { get; set; }
    public double AttendancePercentage { get; set; }
    public int TotalClasses { get; set; }
    public int ClassesAttended { get; set; }
    public Dictionary<string, double> AverageBySubject { get; set; } = new();
}

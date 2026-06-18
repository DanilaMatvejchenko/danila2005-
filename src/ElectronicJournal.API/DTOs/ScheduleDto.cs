namespace ElectronicJournal.API.DTOs;

public class ScheduleDto
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public int TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public DayOfWeek DayOfWeek { get; set; }
    public int PairNumber { get; set; }
    public string Room { get; set; } = string.Empty;
    public string WeekType { get; set; } = string.Empty;
}

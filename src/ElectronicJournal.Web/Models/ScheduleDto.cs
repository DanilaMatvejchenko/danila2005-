namespace ElectronicJournal.Web.Models;

public class ScheduleDto
{
    public int Id { get; set; }
    public int DayOfWeek { get; set; }
    public int LessonNumber { get; set; }
    public string SubjectName { get; set; } = "";
    public string TeacherName { get; set; } = "";
    public string Room { get; set; } = "";
    public string GroupName { get; set; } = "";
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}

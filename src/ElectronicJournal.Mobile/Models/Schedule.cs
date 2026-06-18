namespace ElectronicJournal.Mobile.Models;

public class Schedule
{
    public int Id { get; set; }
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public string Room { get; set; } = string.Empty;
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int LessonNumber { get; set; }
    public int GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;

    public string TimeRange => $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";

    public string DayName => DayOfWeek switch
    {
        DayOfWeek.Monday => "Понедельник",
        DayOfWeek.Tuesday => "Вторник",
        DayOfWeek.Wednesday => "Среда",
        DayOfWeek.Thursday => "Четверг",
        DayOfWeek.Friday => "Пятница",
        DayOfWeek.Saturday => "Суббота",
        DayOfWeek.Sunday => "Воскресенье",
        _ => ""
    };
}

public class ScheduleDay
{
    public string DayName { get; set; } = string.Empty;
    public DayOfWeek DayOfWeek { get; set; }
    public List<Schedule> Lessons { get; set; } = new();
}

namespace ElectronicJournal.Mobile.Models;

public class Attendance
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Note { get; set; }

    public string StatusText => Status switch
    {
        AttendanceStatus.Present => "Присутствует",
        AttendanceStatus.Absent => "Отсутствует",
        AttendanceStatus.Late => "Опоздание",
        AttendanceStatus.Excused => "Уважительная причина",
        _ => "Неизвестно"
    };

    public Color StatusColor => Status switch
    {
        AttendanceStatus.Present => Color.FromArgb("#16a34a"),
        AttendanceStatus.Absent => Color.FromArgb("#dc2626"),
        AttendanceStatus.Late => Color.FromArgb("#f59e0b"),
        AttendanceStatus.Excused => Color.FromArgb("#2563eb"),
        _ => Color.FromArgb("#6b7280")
    };
}

public enum AttendanceStatus
{
    Present = 0,
    Absent = 1,
    Late = 2,
    Excused = 3
}

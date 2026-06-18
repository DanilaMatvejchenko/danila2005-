namespace ElectronicJournal.API.Models;

public enum WeekType
{
    Both,
    Odd,
    Even
}

public class Schedule
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;
    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; } = null!;
    public DayOfWeek DayOfWeek { get; set; }
    public int PairNumber { get; set; }
    public string Room { get; set; } = string.Empty;
    public WeekType WeekType { get; set; }
}

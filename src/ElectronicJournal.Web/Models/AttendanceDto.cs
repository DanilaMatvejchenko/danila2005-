namespace ElectronicJournal.Web.Models;

public class AttendanceDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = "";
    public DateTime Date { get; set; }
    public AttendanceStatus Status { get; set; }
    public string Comment { get; set; } = "";
}

public enum AttendanceStatus
{
    Present,
    Absent,
    Late
}

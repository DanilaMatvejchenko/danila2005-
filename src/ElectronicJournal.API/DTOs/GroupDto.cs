namespace ElectronicJournal.API.DTOs;

public class GroupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CourseNumber { get; set; }
    public string Specialty { get; set; } = string.Empty;
    public int CuratorId { get; set; }
    public string? CuratorName { get; set; }
    public int StudentCount { get; set; }
}

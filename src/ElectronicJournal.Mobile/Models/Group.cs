namespace ElectronicJournal.Mobile.Models;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Speciality { get; set; } = string.Empty;
    public int Course { get; set; }
    public int StudentCount { get; set; }
    public string? CuratorName { get; set; }
}

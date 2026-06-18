namespace ElectronicJournal.Web.Models;

public class GroupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Specialization { get; set; } = "";
    public int Course { get; set; }
    public int StudentCount { get; set; }
    public string CuratorName { get; set; } = "";
}

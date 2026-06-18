namespace ElectronicJournal.API.Models;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CourseNumber { get; set; }
    public string Specialty { get; set; } = string.Empty;
    public int? CuratorId { get; set; }
    public Teacher? Curator { get; set; }
    public ICollection<Student> Students { get; set; } = new List<Student>();
}

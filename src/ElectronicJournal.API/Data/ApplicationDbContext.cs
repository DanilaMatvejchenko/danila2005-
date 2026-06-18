using ElectronicJournal.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<TeacherSubject> TeacherSubjects => Set<TeacherSubject>();
    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Role).HasConversion<string>();
        });

        // Student
        modelBuilder.Entity<Student>(e =>
        {
            e.HasOne(s => s.User).WithMany().HasForeignKey(s => s.UserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(s => s.Parent).WithMany().HasForeignKey(s => s.ParentId).OnDelete(DeleteBehavior.SetNull);
            e.HasOne(s => s.Group).WithMany(g => g.Students).HasForeignKey(s => s.GroupId).OnDelete(DeleteBehavior.Restrict);
        });

        // Teacher
        modelBuilder.Entity<Teacher>(e =>
        {
            e.HasOne(t => t.User).WithMany().HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Restrict);
        });

        // Group
        modelBuilder.Entity<Group>(e =>
        {
            e.HasOne(g => g.Curator).WithMany().HasForeignKey(g => g.CuratorId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(g => g.Name).IsUnique();
        });

        // TeacherSubject - composite key
        modelBuilder.Entity<TeacherSubject>(e =>
        {
            e.HasKey(ts => new { ts.TeacherId, ts.SubjectId, ts.GroupId });
            e.HasOne(ts => ts.Teacher).WithMany().HasForeignKey(ts => ts.TeacherId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(ts => ts.Subject).WithMany().HasForeignKey(ts => ts.SubjectId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(ts => ts.Group).WithMany().HasForeignKey(ts => ts.GroupId).OnDelete(DeleteBehavior.Restrict);
        });

        // Grade
        modelBuilder.Entity<Grade>(e =>
        {
            e.HasOne(g => g.Student).WithMany().HasForeignKey(g => g.StudentId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(g => g.Subject).WithMany().HasForeignKey(g => g.SubjectId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(g => g.Teacher).WithMany().HasForeignKey(g => g.TeacherId).OnDelete(DeleteBehavior.Restrict);
            e.Property(g => g.GradeType).HasConversion<string>();
            e.HasIndex(g => new { g.StudentId, g.SubjectId, g.Date });
        });

        // Attendance
        modelBuilder.Entity<Attendance>(e =>
        {
            e.HasOne(a => a.Student).WithMany().HasForeignKey(a => a.StudentId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(a => a.Subject).WithMany().HasForeignKey(a => a.SubjectId).OnDelete(DeleteBehavior.Restrict);
            e.Property(a => a.Status).HasConversion<string>();
            e.HasIndex(a => new { a.StudentId, a.SubjectId, a.Date });
        });

        // Schedule
        modelBuilder.Entity<Schedule>(e =>
        {
            e.HasOne(s => s.Group).WithMany().HasForeignKey(s => s.GroupId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(s => s.Subject).WithMany().HasForeignKey(s => s.SubjectId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(s => s.Teacher).WithMany().HasForeignKey(s => s.TeacherId).OnDelete(DeleteBehavior.Restrict);
            e.Property(s => s.WeekType).HasConversion<string>();
            e.HasIndex(s => new { s.GroupId, s.DayOfWeek });
        });

        // ChatMessage
        modelBuilder.Entity<ChatMessage>(e =>
        {
            e.HasOne(c => c.Sender).WithMany().HasForeignKey(c => c.SenderId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(c => c.Receiver).WithMany().HasForeignKey(c => c.ReceiverId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(c => new { c.SenderId, c.ReceiverId });
        });

        // Notification
        modelBuilder.Entity<Notification>(e =>
        {
            e.HasOne(n => n.User).WithMany().HasForeignKey(n => n.UserId).OnDelete(DeleteBehavior.Cascade);
            e.Property(n => n.Type).HasConversion<string>();
            e.HasIndex(n => new { n.UserId, n.IsRead });
        });
    }
}

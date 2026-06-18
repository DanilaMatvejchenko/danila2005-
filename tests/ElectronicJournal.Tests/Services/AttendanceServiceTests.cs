using ElectronicJournal.API.Data;
using ElectronicJournal.API.Models;
using ElectronicJournal.API.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace ElectronicJournal.Tests.Services;

public class AttendanceServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly AttendanceService _service;

    public AttendanceServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);
        var logger = new Mock<ILogger<AttendanceService>>();
        _service = new AttendanceService(_context, logger.Object);
        SeedData();
    }

    private void SeedData()
    {
        var teacherUser = new User { Id = 1, Email = "t@t.ru", PasswordHash = "h", FirstName = "A", LastName = "B", Role = UserRole.Teacher };
        var studentUser = new User { Id = 2, Email = "s@t.ru", PasswordHash = "h", FirstName = "C", LastName = "D", Role = UserRole.Student };
        _context.Users.AddRange(teacherUser, studentUser);

        _context.Groups.Add(new Group { Id = 1, Name = "ИС-21", CourseNumber = 2, Specialty = "ИС" });
        _context.Teachers.Add(new Teacher { Id = 1, UserId = 1, Department = "ИТ" });
        _context.Students.Add(new Student { Id = 1, UserId = 2, GroupId = 1 });
        _context.Subjects.Add(new Subject { Id = 1, Name = "Мат", HoursTotal = 120 });

        _context.Attendances.AddRange(
            new Attendance { Id = 1, StudentId = 1, SubjectId = 1, Date = DateTime.Today, Status = AttendanceStatus.Present, MarkedById = 1 },
            new Attendance { Id = 2, StudentId = 1, SubjectId = 1, Date = DateTime.Today.AddDays(-1), Status = AttendanceStatus.Absent, MarkedById = 1 },
            new Attendance { Id = 3, StudentId = 1, SubjectId = 1, Date = DateTime.Today.AddDays(-2), Status = AttendanceStatus.Present, MarkedById = 1 }
        );

        _context.SaveChanges();
    }

    [Fact]
    public async Task GetByStudentId_ReturnsAll()
    {
        var records = await _service.GetByStudentIdAsync(1);
        records.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetStatistics_CalculatesCorrectly()
    {
        var stats = await _service.GetStatisticsAsync(1);
        stats.TotalClasses.Should().Be(3);
        stats.PresentCount.Should().Be(2);
        stats.AbsentCount.Should().Be(1);
        stats.AttendancePercentage.Should().BeApproximately(66.67, 0.01);
    }

    [Fact]
    public async Task Create_AddsRecord()
    {
        var record = new Attendance
        {
            StudentId = 1, SubjectId = 1,
            Date = DateTime.Today.AddDays(-5),
            Status = AttendanceStatus.Late, MarkedById = 1
        };

        var result = await _service.CreateAsync(record);
        result.Id.Should().BeGreaterThan(0);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

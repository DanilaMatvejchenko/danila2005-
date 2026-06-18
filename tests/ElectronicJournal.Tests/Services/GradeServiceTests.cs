using ElectronicJournal.API.Data;
using ElectronicJournal.API.Models;
using ElectronicJournal.API.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace ElectronicJournal.Tests.Services;

public class GradeServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly GradeService _service;

    public GradeServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);
        var logger = new Mock<ILogger<GradeService>>();
        _service = new GradeService(_context, logger.Object);
        SeedData();
    }

    private void SeedData()
    {
        var teacherUser = new User { Id = 1, Email = "teacher@test.ru", PasswordHash = "hash", FirstName = "Иван", LastName = "Петров", Role = UserRole.Teacher };
        var studentUser = new User { Id = 2, Email = "student@test.ru", PasswordHash = "hash", FirstName = "Мария", LastName = "Иванова", Role = UserRole.Student };
        _context.Users.AddRange(teacherUser, studentUser);

        var group = new Group { Id = 1, Name = "ИС-21", CourseNumber = 2, Specialty = "Информационные системы" };
        _context.Groups.Add(group);

        var teacher = new Teacher { Id = 1, UserId = 1, Department = "ИТ" };
        _context.Teachers.Add(teacher);

        var student = new Student { Id = 1, UserId = 2, GroupId = 1 };
        _context.Students.Add(student);

        var subject = new Subject { Id = 1, Name = "Математика", HoursTotal = 120 };
        _context.Subjects.Add(subject);

        _context.Grades.AddRange(
            new Grade { Id = 1, StudentId = 1, SubjectId = 1, TeacherId = 1, Value = 5, Date = DateTime.Today, GradeType = GradeType.Current },
            new Grade { Id = 2, StudentId = 1, SubjectId = 1, TeacherId = 1, Value = 4, Date = DateTime.Today.AddDays(-1), GradeType = GradeType.Current },
            new Grade { Id = 3, StudentId = 1, SubjectId = 1, TeacherId = 1, Value = 3, Date = DateTime.Today.AddDays(-2), GradeType = GradeType.Current }
        );

        _context.SaveChanges();
    }

    [Fact]
    public async Task GetByStudentId_ReturnsAllGrades()
    {
        var grades = await _service.GetByStudentIdAsync(1);
        grades.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetByStudentId_ReturnsEmpty_WhenNoGrades()
    {
        var grades = await _service.GetByStudentIdAsync(999);
        grades.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAverageGrade_CalculatesCorrectly()
    {
        var avg = await _service.GetAverageGradeAsync(1, 1);
        avg.Should().Be(4.0);
    }

    [Fact]
    public async Task CreateGrade_AddsToDatabase()
    {
        var grade = new Grade
        {
            StudentId = 1, SubjectId = 1, TeacherId = 1,
            Value = 5, Date = DateTime.Today, GradeType = GradeType.Midterm
        };

        var result = await _service.CreateAsync(grade);

        result.Id.Should().BeGreaterThan(0);
        var count = await _context.Grades.CountAsync();
        count.Should().Be(4);
    }

    [Fact]
    public async Task GetBySubjectId_ReturnsCorrectGrades()
    {
        var grades = await _service.GetBySubjectIdAsync(1);
        grades.Should().HaveCount(3);
        grades.Should().AllSatisfy(g => g.SubjectId.Should().Be(1));
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

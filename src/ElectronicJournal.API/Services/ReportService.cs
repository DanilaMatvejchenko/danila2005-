using System.Text;
using ElectronicJournal.API.Data;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.API.Services;

public class ReportService : IReportService
{
    private readonly ApplicationDbContext _db;

    public ReportService(ApplicationDbContext db) => _db = db;

    public async Task<byte[]> GenerateStudentReportAsync(int studentId)
    {
        var student = await _db.Students
            .Include(s => s.User)
            .Include(s => s.Group)
            .FirstOrDefaultAsync(s => s.Id == studentId)
            ?? throw new KeyNotFoundException("Студент не найден.");

        var grades = await _db.Grades
            .Include(g => g.Subject)
            .Where(g => g.StudentId == studentId)
            .ToListAsync();

        var attendances = await _db.Attendances
            .Where(a => a.StudentId == studentId)
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("=== ОТЧЕТ ПО СТУДЕНТУ ===");
        sb.AppendLine($"ФИО: {student.User.LastName} {student.User.FirstName} {student.User.Patronymic}");
        sb.AppendLine($"Группа: {student.Group.Name}");
        sb.AppendLine($"Курс: {student.Group.CourseNumber}");
        sb.AppendLine($"Специальность: {student.Group.Specialty}");
        sb.AppendLine();

        sb.AppendLine("--- Оценки ---");
        foreach (var group in grades.GroupBy(g => g.Subject.Name))
        {
            sb.AppendLine($"  {group.Key}: {string.Join(", ", group.Select(g => g.Value))} (Средний: {group.Average(g => g.Value):F2})");
        }

        sb.AppendLine();
        sb.AppendLine("--- Посещаемость ---");
        var total = attendances.Count;
        var present = attendances.Count(a => a.Status == Models.AttendanceStatus.Present || a.Status == Models.AttendanceStatus.Late);
        sb.AppendLine($"  Всего занятий: {total}");
        sb.AppendLine($"  Присутствовал: {present}");
        sb.AppendLine($"  Процент посещаемости: {(total > 0 ? (double)present / total * 100 : 0):F1}%");

        if (grades.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine($"Общий средний балл: {grades.Average(g => g.Value):F2}");
        }

        sb.AppendLine();
        sb.AppendLine($"Дата формирования: {DateTime.UtcNow:dd.MM.yyyy HH:mm}");

        return Encoding.UTF8.GetBytes(sb.ToString());
    }
}

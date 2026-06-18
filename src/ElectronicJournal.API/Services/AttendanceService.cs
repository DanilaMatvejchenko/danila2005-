using ElectronicJournal.API.Data;
using ElectronicJournal.API.DTOs;
using ElectronicJournal.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.API.Services;

public class AttendanceService : IAttendanceService
{
    private readonly ApplicationDbContext _db;

    public AttendanceService(ApplicationDbContext db) => _db = db;

    public async Task<AttendanceResponseDto> CreateAsync(CreateAttendanceDto dto)
    {
        var entity = new Attendance
        {
            StudentId = dto.StudentId,
            SubjectId = dto.SubjectId,
            Date = dto.Date,
            Status = dto.Status,
            Note = dto.Note
        };
        _db.Attendances.Add(entity);
        await _db.SaveChangesAsync();
        return await MapToDto(entity.Id);
    }

    public async Task<AttendanceResponseDto?> GetByIdAsync(int id)
    {
        var a = await Query().FirstOrDefaultAsync(x => x.Id == id);
        return a is null ? null : ToDto(a);
    }

    public async Task<IEnumerable<AttendanceResponseDto>> GetByStudentAsync(int studentId) =>
        (await Query().Where(a => a.StudentId == studentId).ToListAsync()).Select(ToDto);

    public async Task<StudentStatsDto> GetStatisticsAsync(int studentId)
    {
        var student = await _db.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == studentId)
            ?? throw new KeyNotFoundException("Студент не найден.");

        var attendances = await _db.Attendances.Where(a => a.StudentId == studentId).ToListAsync();
        var grades = await _db.Grades.Include(g => g.Subject).Where(g => g.StudentId == studentId).ToListAsync();

        var total = attendances.Count;
        var attended = attendances.Count(a => a.Status == AttendanceStatus.Present || a.Status == AttendanceStatus.Late);

        return new StudentStatsDto
        {
            StudentId = studentId,
            StudentName = $"{student.User.LastName} {student.User.FirstName}",
            AverageGrade = grades.Count > 0 ? grades.Average(g => g.Value) : 0,
            AttendancePercentage = total > 0 ? Math.Round((double)attended / total * 100, 2) : 0,
            TotalClasses = total,
            ClassesAttended = attended,
            AverageBySubject = grades.GroupBy(g => g.Subject.Name)
                .ToDictionary(g => g.Key, g => Math.Round(g.Average(x => x.Value), 2))
        };
    }

    public async Task<AttendanceResponseDto> UpdateAsync(int id, CreateAttendanceDto dto)
    {
        var entity = await _db.Attendances.FindAsync(id) ?? throw new KeyNotFoundException("Запись не найдена.");
        entity.StudentId = dto.StudentId;
        entity.SubjectId = dto.SubjectId;
        entity.Date = dto.Date;
        entity.Status = dto.Status;
        entity.Note = dto.Note;
        await _db.SaveChangesAsync();
        return await MapToDto(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Attendances.FindAsync(id);
        if (entity is null) return false;
        _db.Attendances.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }

    private IQueryable<Attendance> Query() =>
        _db.Attendances.Include(a => a.Student).ThenInclude(s => s.User).Include(a => a.Subject);

    private async Task<AttendanceResponseDto> MapToDto(int id) =>
        ToDto(await Query().FirstAsync(a => a.Id == id));

    private static AttendanceResponseDto ToDto(Attendance a) => new()
    {
        Id = a.Id,
        StudentId = a.StudentId,
        StudentName = $"{a.Student.User.LastName} {a.Student.User.FirstName}",
        SubjectId = a.SubjectId,
        SubjectName = a.Subject.Name,
        Date = a.Date,
        Status = a.Status.ToString(),
        Note = a.Note
    };
}

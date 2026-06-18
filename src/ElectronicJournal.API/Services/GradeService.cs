using ElectronicJournal.API.Data;
using ElectronicJournal.API.DTOs;
using ElectronicJournal.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.API.Services;

public class GradeService : IGradeService
{
    private readonly ApplicationDbContext _db;

    public GradeService(ApplicationDbContext db) => _db = db;

    public async Task<GradeResponseDto> CreateAsync(CreateGradeDto dto)
    {
        var grade = new Grade
        {
            StudentId = dto.StudentId,
            SubjectId = dto.SubjectId,
            TeacherId = dto.TeacherId,
            Value = dto.Value,
            Date = dto.Date,
            Comment = dto.Comment,
            GradeType = dto.GradeType
        };
        _db.Grades.Add(grade);
        await _db.SaveChangesAsync();
        return await MapToDto(grade.Id);
    }

    public async Task<GradeResponseDto?> GetByIdAsync(int id)
    {
        var grade = await QueryGrades().FirstOrDefaultAsync(g => g.Id == id);
        return grade is null ? null : ToDto(grade);
    }

    public async Task<IEnumerable<GradeResponseDto>> GetByStudentAsync(int studentId) =>
        (await QueryGrades().Where(g => g.StudentId == studentId).ToListAsync()).Select(ToDto);

    public async Task<IEnumerable<GradeResponseDto>> GetBySubjectAsync(int subjectId) =>
        (await QueryGrades().Where(g => g.SubjectId == subjectId).ToListAsync()).Select(ToDto);

    public async Task<double> GetAverageGradeAsync(int studentId, int? subjectId = null)
    {
        var query = _db.Grades.Where(g => g.StudentId == studentId);
        if (subjectId.HasValue) query = query.Where(g => g.SubjectId == subjectId.Value);
        var avg = await query.AverageAsync(g => (double?)g.Value);
        return avg ?? 0;
    }

    public async Task<GradeResponseDto> UpdateAsync(int id, CreateGradeDto dto)
    {
        var grade = await _db.Grades.FindAsync(id) ?? throw new KeyNotFoundException("Оценка не найдена.");
        grade.StudentId = dto.StudentId;
        grade.SubjectId = dto.SubjectId;
        grade.TeacherId = dto.TeacherId;
        grade.Value = dto.Value;
        grade.Date = dto.Date;
        grade.Comment = dto.Comment;
        grade.GradeType = dto.GradeType;
        await _db.SaveChangesAsync();
        return await MapToDto(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var grade = await _db.Grades.FindAsync(id);
        if (grade is null) return false;
        _db.Grades.Remove(grade);
        await _db.SaveChangesAsync();
        return true;
    }

    private IQueryable<Grade> QueryGrades() =>
        _db.Grades.Include(g => g.Student).ThenInclude(s => s.User)
            .Include(g => g.Subject)
            .Include(g => g.Teacher).ThenInclude(t => t.User);

    private async Task<GradeResponseDto> MapToDto(int id) =>
        ToDto((await QueryGrades().FirstAsync(g => g.Id == id)));

    private static GradeResponseDto ToDto(Grade g) => new()
    {
        Id = g.Id,
        StudentId = g.StudentId,
        StudentName = $"{g.Student.User.LastName} {g.Student.User.FirstName}",
        SubjectId = g.SubjectId,
        SubjectName = g.Subject.Name,
        TeacherId = g.TeacherId,
        TeacherName = $"{g.Teacher.User.LastName} {g.Teacher.User.FirstName}",
        Value = g.Value,
        Date = g.Date,
        Comment = g.Comment,
        GradeType = g.GradeType.ToString()
    };
}

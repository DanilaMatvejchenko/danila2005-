using ElectronicJournal.API.DTOs;

namespace ElectronicJournal.API.Services;

public interface IGradeService
{
    Task<GradeResponseDto> CreateAsync(CreateGradeDto dto);
    Task<GradeResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<GradeResponseDto>> GetByStudentAsync(int studentId);
    Task<IEnumerable<GradeResponseDto>> GetBySubjectAsync(int subjectId);
    Task<double> GetAverageGradeAsync(int studentId, int? subjectId = null);
    Task<GradeResponseDto> UpdateAsync(int id, CreateGradeDto dto);
    Task<bool> DeleteAsync(int id);
}

using ElectronicJournal.API.DTOs;

namespace ElectronicJournal.API.Services;

public interface IAttendanceService
{
    Task<AttendanceResponseDto> CreateAsync(CreateAttendanceDto dto);
    Task<AttendanceResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<AttendanceResponseDto>> GetByStudentAsync(int studentId);
    Task<StudentStatsDto> GetStatisticsAsync(int studentId);
    Task<AttendanceResponseDto> UpdateAsync(int id, CreateAttendanceDto dto);
    Task<bool> DeleteAsync(int id);
}

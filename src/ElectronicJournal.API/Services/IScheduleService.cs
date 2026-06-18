using ElectronicJournal.API.DTOs;

namespace ElectronicJournal.API.Services;

public interface IScheduleService
{
    Task<IEnumerable<ScheduleDto>> GetByGroupAsync(int groupId);
    Task<IEnumerable<ScheduleDto>> GetByTeacherAsync(int teacherId);
    Task<IEnumerable<ScheduleDto>> GetForTodayAsync(int groupId);
}

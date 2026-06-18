using ElectronicJournal.API.Data;
using ElectronicJournal.API.DTOs;
using ElectronicJournal.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.API.Services;

public class ScheduleService : IScheduleService
{
    private readonly ApplicationDbContext _db;

    public ScheduleService(ApplicationDbContext db) => _db = db;

    public async Task<IEnumerable<ScheduleDto>> GetByGroupAsync(int groupId) =>
        (await Query().Where(s => s.GroupId == groupId).OrderBy(s => s.DayOfWeek).ThenBy(s => s.PairNumber).ToListAsync()).Select(ToDto);

    public async Task<IEnumerable<ScheduleDto>> GetByTeacherAsync(int teacherId) =>
        (await Query().Where(s => s.TeacherId == teacherId).OrderBy(s => s.DayOfWeek).ThenBy(s => s.PairNumber).ToListAsync()).Select(ToDto);

    public async Task<IEnumerable<ScheduleDto>> GetForTodayAsync(int groupId)
    {
        var today = DateTime.Now.DayOfWeek;
        var weekNumber = System.Globalization.ISOWeek.GetWeekOfYear(DateTime.Now);
        var isOdd = weekNumber % 2 != 0;

        var schedules = await Query()
            .Where(s => s.GroupId == groupId && s.DayOfWeek == today)
            .Where(s => s.WeekType == WeekType.Both || (isOdd ? s.WeekType == WeekType.Odd : s.WeekType == WeekType.Even))
            .OrderBy(s => s.PairNumber)
            .ToListAsync();

        return schedules.Select(ToDto);
    }

    private IQueryable<Schedule> Query() =>
        _db.Schedules.Include(s => s.Group).Include(s => s.Subject).Include(s => s.Teacher).ThenInclude(t => t.User);

    private static ScheduleDto ToDto(Schedule s) => new()
    {
        Id = s.Id,
        GroupId = s.GroupId,
        GroupName = s.Group.Name,
        SubjectId = s.SubjectId,
        SubjectName = s.Subject.Name,
        TeacherId = s.TeacherId,
        TeacherName = $"{s.Teacher.User.LastName} {s.Teacher.User.FirstName}",
        DayOfWeek = s.DayOfWeek,
        PairNumber = s.PairNumber,
        Room = s.Room,
        WeekType = s.WeekType.ToString()
    };
}

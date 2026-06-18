using ElectronicJournal.API.DTOs;
using ElectronicJournal.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.API.Controllers;

/// <summary>
/// Контроллер расписания занятий.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleService _service;

    public ScheduleController(IScheduleService service) => _service = service;

    /// <summary>
    /// Получить расписание группы.
    /// </summary>
    [HttpGet("group/{groupId}")]
    [ProducesResponseType(typeof(IEnumerable<ScheduleDto>), 200)]
    public async Task<IActionResult> GetByGroup(int groupId) =>
        Ok(await _service.GetByGroupAsync(groupId));

    /// <summary>
    /// Получить расписание преподавателя.
    /// </summary>
    [HttpGet("teacher/{teacherId}")]
    [ProducesResponseType(typeof(IEnumerable<ScheduleDto>), 200)]
    public async Task<IActionResult> GetByTeacher(int teacherId) =>
        Ok(await _service.GetByTeacherAsync(teacherId));

    /// <summary>
    /// Получить расписание на сегодня для группы.
    /// </summary>
    [HttpGet("today/{groupId}")]
    [ProducesResponseType(typeof(IEnumerable<ScheduleDto>), 200)]
    public async Task<IActionResult> GetForToday(int groupId) =>
        Ok(await _service.GetForTodayAsync(groupId));
}

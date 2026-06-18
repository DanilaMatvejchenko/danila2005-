using ElectronicJournal.API.DTOs;
using ElectronicJournal.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.API.Controllers;

/// <summary>
/// Контроллер управления посещаемостью.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _service;

    public AttendanceController(IAttendanceService service) => _service = service;

    /// <summary>
    /// Создать запись посещаемости.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(typeof(AttendanceResponseDto), 201)]
    public async Task<IActionResult> Create([FromBody] CreateAttendanceDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Получить запись по ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AttendanceResponseDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Получить посещаемость студента.
    /// </summary>
    [HttpGet("student/{studentId}")]
    [ProducesResponseType(typeof(IEnumerable<AttendanceResponseDto>), 200)]
    public async Task<IActionResult> GetByStudent(int studentId) =>
        Ok(await _service.GetByStudentAsync(studentId));

    /// <summary>
    /// Получить статистику студента.
    /// </summary>
    [HttpGet("statistics/{studentId}")]
    [ProducesResponseType(typeof(StudentStatsDto), 200)]
    public async Task<IActionResult> GetStatistics(int studentId) =>
        Ok(await _service.GetStatisticsAsync(studentId));

    /// <summary>
    /// Обновить запись посещаемости.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(typeof(AttendanceResponseDto), 200)]
    public async Task<IActionResult> Update(int id, [FromBody] CreateAttendanceDto dto) =>
        Ok(await _service.UpdateAsync(id, dto));

    /// <summary>
    /// Удалить запись посещаемости.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id) =>
        await _service.DeleteAsync(id) ? NoContent() : NotFound();
}

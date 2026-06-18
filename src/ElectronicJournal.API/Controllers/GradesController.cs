using ElectronicJournal.API.DTOs;
using ElectronicJournal.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.API.Controllers;

/// <summary>
/// Контроллер управления оценками.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GradesController : ControllerBase
{
    private readonly IGradeService _gradeService;

    public GradesController(IGradeService gradeService) => _gradeService = gradeService;

    /// <summary>
    /// Создать новую оценку.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(typeof(GradeResponseDto), 201)]
    public async Task<IActionResult> Create([FromBody] CreateGradeDto dto)
    {
        var result = await _gradeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Получить оценку по ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GradeResponseDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _gradeService.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Получить оценки студента.
    /// </summary>
    [HttpGet("student/{studentId}")]
    [ProducesResponseType(typeof(IEnumerable<GradeResponseDto>), 200)]
    public async Task<IActionResult> GetByStudent(int studentId) =>
        Ok(await _gradeService.GetByStudentAsync(studentId));

    /// <summary>
    /// Получить оценки по предмету.
    /// </summary>
    [HttpGet("subject/{subjectId}")]
    [ProducesResponseType(typeof(IEnumerable<GradeResponseDto>), 200)]
    public async Task<IActionResult> GetBySubject(int subjectId) =>
        Ok(await _gradeService.GetBySubjectAsync(subjectId));

    /// <summary>
    /// Получить средний балл студента.
    /// </summary>
    [HttpGet("average/{studentId}")]
    [ProducesResponseType(typeof(double), 200)]
    public async Task<IActionResult> GetAverage(int studentId, [FromQuery] int? subjectId = null) =>
        Ok(await _gradeService.GetAverageGradeAsync(studentId, subjectId));

    /// <summary>
    /// Обновить оценку.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(typeof(GradeResponseDto), 200)]
    public async Task<IActionResult> Update(int id, [FromBody] CreateGradeDto dto) =>
        Ok(await _gradeService.UpdateAsync(id, dto));

    /// <summary>
    /// Удалить оценку.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id) =>
        await _gradeService.DeleteAsync(id) ? NoContent() : NotFound();
}

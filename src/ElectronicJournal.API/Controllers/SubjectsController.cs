using ElectronicJournal.API.Data;
using ElectronicJournal.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.API.Controllers;

/// <summary>
/// Контроллер управления предметами.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubjectsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public SubjectsController(ApplicationDbContext db) => _db = db;

    /// <summary>
    /// Получить все предметы.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Subject>), 200)]
    public async Task<IActionResult> GetAll() => Ok(await _db.Subjects.ToListAsync());

    /// <summary>
    /// Получить предмет по ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Subject), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
        var subject = await _db.Subjects.FindAsync(id);
        return subject is null ? NotFound() : Ok(subject);
    }

    /// <summary>
    /// Создать предмет.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Subject), 201)]
    public async Task<IActionResult> Create([FromBody] Subject subject)
    {
        _db.Subjects.Add(subject);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = subject.Id }, subject);
    }

    /// <summary>
    /// Обновить предмет.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] Subject dto)
    {
        var subject = await _db.Subjects.FindAsync(id);
        if (subject is null) return NotFound();
        subject.Name = dto.Name;
        subject.Description = dto.Description;
        subject.HoursTotal = dto.HoursTotal;
        await _db.SaveChangesAsync();
        return Ok(subject);
    }

    /// <summary>
    /// Удалить предмет.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        var subject = await _db.Subjects.FindAsync(id);
        if (subject is null) return NotFound();
        _db.Subjects.Remove(subject);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

using ElectronicJournal.API.Data;
using ElectronicJournal.API.DTOs;
using ElectronicJournal.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.API.Controllers;

/// <summary>
/// Контроллер управления группами.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public GroupsController(ApplicationDbContext db) => _db = db;

    /// <summary>
    /// Получить все группы.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GroupDto>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var groups = await _db.Groups
            .Include(g => g.Curator).ThenInclude(t => t.User)
            .Include(g => g.Students)
            .ToListAsync();

        return Ok(groups.Select(g => new GroupDto
        {
            Id = g.Id,
            Name = g.Name,
            CourseNumber = g.CourseNumber,
            Specialty = g.Specialty,
            CuratorId = g.CuratorId,
            CuratorName = $"{g.Curator.User.LastName} {g.Curator.User.FirstName}",
            StudentCount = g.Students.Count
        }));
    }

    /// <summary>
    /// Получить группу по ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GroupDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
        var g = await _db.Groups
            .Include(x => x.Curator).ThenInclude(t => t.User)
            .Include(x => x.Students)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (g is null) return NotFound();

        return Ok(new GroupDto
        {
            Id = g.Id, Name = g.Name, CourseNumber = g.CourseNumber,
            Specialty = g.Specialty, CuratorId = g.CuratorId,
            CuratorName = $"{g.Curator.User.LastName} {g.Curator.User.FirstName}",
            StudentCount = g.Students.Count
        });
    }

    /// <summary>
    /// Создать группу.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(GroupDto), 201)]
    public async Task<IActionResult> Create([FromBody] GroupDto dto)
    {
        var group = new Group
        {
            Name = dto.Name,
            CourseNumber = dto.CourseNumber,
            Specialty = dto.Specialty,
            CuratorId = dto.CuratorId
        };
        _db.Groups.Add(group);
        await _db.SaveChangesAsync();
        dto.Id = group.Id;
        return CreatedAtAction(nameof(GetById), new { id = group.Id }, dto);
    }

    /// <summary>
    /// Обновить группу.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] GroupDto dto)
    {
        var group = await _db.Groups.FindAsync(id);
        if (group is null) return NotFound();
        group.Name = dto.Name;
        group.CourseNumber = dto.CourseNumber;
        group.Specialty = dto.Specialty;
        group.CuratorId = dto.CuratorId;
        await _db.SaveChangesAsync();
        dto.Id = id;
        return Ok(dto);
    }

    /// <summary>
    /// Удалить группу.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        var group = await _db.Groups.FindAsync(id);
        if (group is null) return NotFound();
        _db.Groups.Remove(group);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

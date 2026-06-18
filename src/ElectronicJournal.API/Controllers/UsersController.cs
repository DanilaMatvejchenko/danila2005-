using System.Security.Claims;
using ElectronicJournal.API.Data;
using ElectronicJournal.API.DTOs;
using ElectronicJournal.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.API.Controllers;

/// <summary>
/// Контроллер управления пользователями.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public UsersController(ApplicationDbContext db) => _db = db;

    /// <summary>
    /// Получить профиль текущего пользователя.
    /// </summary>
    [HttpGet("profile")]
    [ProducesResponseType(typeof(UserProfileDto), 200)]
    public async Task<IActionResult> GetProfile()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _db.Users.FindAsync(userId);
        if (user is null) return NotFound();
        return Ok(ToDto(user));
    }

    /// <summary>
    /// Обновить профиль текущего пользователя.
    /// </summary>
    [HttpPut("profile")]
    [ProducesResponseType(typeof(UserProfileDto), 200)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _db.Users.FindAsync(userId);
        if (user is null) return NotFound();

        if (dto.FirstName is not null) user.FirstName = dto.FirstName;
        if (dto.LastName is not null) user.LastName = dto.LastName;
        if (dto.Patronymic is not null) user.Patronymic = dto.Patronymic;
        if (dto.Phone is not null) user.Phone = dto.Phone;
        await _db.SaveChangesAsync();

        return Ok(ToDto(user));
    }

    /// <summary>
    /// Получить пользователей по роли.
    /// </summary>
    [HttpGet("role/{role}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(IEnumerable<UserProfileDto>), 200)]
    public async Task<IActionResult> GetByRole(UserRole role)
    {
        var users = await _db.Users.Where(u => u.Role == role).ToListAsync();
        return Ok(users.Select(ToDto));
    }

    private static UserProfileDto ToDto(User u) => new()
    {
        Id = u.Id, Email = u.Email, FirstName = u.FirstName, LastName = u.LastName,
        Patronymic = u.Patronymic, Role = u.Role.ToString(), Phone = u.Phone, CreatedAt = u.CreatedAt
    };
}

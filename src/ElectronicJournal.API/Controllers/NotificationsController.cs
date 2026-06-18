using System.Security.Claims;
using ElectronicJournal.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.API.Controllers;

/// <summary>
/// Контроллер уведомлений.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _service;

    public NotificationsController(INotificationService service) => _service = service;

    /// <summary>
    /// Получить уведомления текущего пользователя.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await _service.GetForUserAsync(userId));
    }

    /// <summary>
    /// Отметить уведомление как прочитанное.
    /// </summary>
    [HttpPut("{id}/read")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        await _service.MarkAsReadAsync(id);
        return NoContent();
    }
}

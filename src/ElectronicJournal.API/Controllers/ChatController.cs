using System.Security.Claims;
using ElectronicJournal.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.API.Controllers;

/// <summary>
/// Контроллер чата между пользователями.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService) => _chatService = chatService;

    /// <summary>
    /// Отправить сообщение.
    /// </summary>
    [HttpPost("send")]
    [ProducesResponseType(201)]
    public async Task<IActionResult> Send([FromBody] SendMessageRequest request)
    {
        var senderId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var message = await _chatService.SendMessageAsync(senderId, request.ReceiverId, request.Text);
        return CreatedAtAction(null, new { id = message.Id }, message);
    }

    /// <summary>
    /// Получить переписку с пользователем.
    /// </summary>
    [HttpGet("conversation/{otherUserId}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetConversation(int otherUserId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await _chatService.GetConversationAsync(userId, otherUserId));
    }

    /// <summary>
    /// Отметить сообщение как прочитанное.
    /// </summary>
    [HttpPut("read/{messageId}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> MarkAsRead(int messageId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _chatService.MarkAsReadAsync(messageId, userId);
        return NoContent();
    }
}

public class SendMessageRequest
{
    public int ReceiverId { get; set; }
    public string Text { get; set; } = string.Empty;
}

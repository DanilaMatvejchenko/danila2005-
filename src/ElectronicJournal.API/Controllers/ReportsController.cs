using ElectronicJournal.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.API.Controllers;

/// <summary>
/// Контроллер формирования отчетов.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Teacher")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _service;

    public ReportsController(IReportService service) => _service = service;

    /// <summary>
    /// Сгенерировать отчет по студенту.
    /// </summary>
    [HttpGet("student/{studentId}")]
    [ProducesResponseType(typeof(FileContentResult), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetStudentReport(int studentId)
    {
        try
        {
            var data = await _service.GenerateStudentReportAsync(studentId);
            return File(data, "application/octet-stream", $"report_student_{studentId}.txt");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}

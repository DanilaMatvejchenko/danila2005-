using System.Net.Http.Headers;
using System.Net.Http.Json;
using ElectronicJournal.Mobile.Models;

namespace ElectronicJournal.Mobile.Services;

public interface IApiService
{
    // Grades
    Task<List<Grade>> GetGradesAsync(int? studentId = null, int? subjectId = null);
    Task<List<GradeGroup>> GetGradesGroupedAsync(int? studentId = null);
    Task<Grade?> CreateGradeAsync(Grade grade);

    // Schedule
    Task<List<Schedule>> GetScheduleAsync(int? groupId = null);
    Task<List<ScheduleDay>> GetWeeklyScheduleAsync(int? groupId = null);

    // Attendance
    Task<List<Attendance>> GetAttendanceAsync(int? studentId = null, DateTime? from = null, DateTime? to = null);
    Task<Attendance?> MarkAttendanceAsync(Attendance attendance);

    // Chat
    Task<List<ChatMessage>> GetMessagesAsync(int? recipientId = null, int? groupChatId = null);
    Task<ChatMessage?> SendMessageAsync(ChatMessage message);

    // Users
    Task<User?> GetUserAsync(int id);
    Task<User?> UpdateProfileAsync(User user);
    Task<List<User>> GetUsersAsync(string? role = null);

    // Groups
    Task<List<Group>> GetGroupsAsync();

    // Dashboard
    Task<DashboardStats> GetDashboardStatsAsync();
}

public class DashboardStats
{
    public double AverageGrade { get; set; }
    public double AttendancePercent { get; set; }
    public int TodayLessonsCount { get; set; }
    public List<Grade> RecentGrades { get; set; } = new();
    public List<Schedule> TodaySchedule { get; set; } = new();
}

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthService _authService;

    public ApiService(HttpClient httpClient, IAuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    private async Task SetAuthHeaderAsync()
    {
        var token = await _authService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    // Grades
    public async Task<List<Grade>> GetGradesAsync(int? studentId = null, int? subjectId = null)
    {
        await SetAuthHeaderAsync();
        var query = "api/grades?";
        if (studentId.HasValue) query += $"studentId={studentId}&";
        if (subjectId.HasValue) query += $"subjectId={subjectId}&";
        return await _httpClient.GetFromJsonAsync<List<Grade>>(query) ?? new();
    }

    public async Task<List<GradeGroup>> GetGradesGroupedAsync(int? studentId = null)
    {
        await SetAuthHeaderAsync();
        var query = studentId.HasValue ? $"api/grades/grouped?studentId={studentId}" : "api/grades/grouped";
        return await _httpClient.GetFromJsonAsync<List<GradeGroup>>(query) ?? new();
    }

    public async Task<Grade?> CreateGradeAsync(Grade grade)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/grades", grade);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Grade>() : null;
    }

    // Schedule
    public async Task<List<Schedule>> GetScheduleAsync(int? groupId = null)
    {
        await SetAuthHeaderAsync();
        var query = groupId.HasValue ? $"api/schedule?groupId={groupId}" : "api/schedule";
        return await _httpClient.GetFromJsonAsync<List<Schedule>>(query) ?? new();
    }

    public async Task<List<ScheduleDay>> GetWeeklyScheduleAsync(int? groupId = null)
    {
        await SetAuthHeaderAsync();
        var query = groupId.HasValue ? $"api/schedule/weekly?groupId={groupId}" : "api/schedule/weekly";
        return await _httpClient.GetFromJsonAsync<List<ScheduleDay>>(query) ?? new();
    }

    // Attendance
    public async Task<List<Attendance>> GetAttendanceAsync(int? studentId = null, DateTime? from = null, DateTime? to = null)
    {
        await SetAuthHeaderAsync();
        var query = "api/attendance?";
        if (studentId.HasValue) query += $"studentId={studentId}&";
        if (from.HasValue) query += $"from={from:yyyy-MM-dd}&";
        if (to.HasValue) query += $"to={to:yyyy-MM-dd}&";
        return await _httpClient.GetFromJsonAsync<List<Attendance>>(query) ?? new();
    }

    public async Task<Attendance?> MarkAttendanceAsync(Attendance attendance)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/attendance", attendance);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Attendance>() : null;
    }

    // Chat
    public async Task<List<ChatMessage>> GetMessagesAsync(int? recipientId = null, int? groupChatId = null)
    {
        await SetAuthHeaderAsync();
        var query = "api/chat/messages?";
        if (recipientId.HasValue) query += $"recipientId={recipientId}&";
        if (groupChatId.HasValue) query += $"groupChatId={groupChatId}&";
        return await _httpClient.GetFromJsonAsync<List<ChatMessage>>(query) ?? new();
    }

    public async Task<ChatMessage?> SendMessageAsync(ChatMessage message)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/chat/messages", message);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<ChatMessage>() : null;
    }

    // Users
    public async Task<User?> GetUserAsync(int id)
    {
        await SetAuthHeaderAsync();
        return await _httpClient.GetFromJsonAsync<User>($"api/users/{id}");
    }

    public async Task<User?> UpdateProfileAsync(User user)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync($"api/users/{user.Id}", user);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<User>() : null;
    }

    public async Task<List<User>> GetUsersAsync(string? role = null)
    {
        await SetAuthHeaderAsync();
        var query = string.IsNullOrEmpty(role) ? "api/users" : $"api/users?role={role}";
        return await _httpClient.GetFromJsonAsync<List<User>>(query) ?? new();
    }

    // Groups
    public async Task<List<Group>> GetGroupsAsync()
    {
        await SetAuthHeaderAsync();
        return await _httpClient.GetFromJsonAsync<List<Group>>("api/groups") ?? new();
    }

    // Dashboard
    public async Task<DashboardStats> GetDashboardStatsAsync()
    {
        await SetAuthHeaderAsync();
        return await _httpClient.GetFromJsonAsync<DashboardStats>("api/dashboard") ?? new();
    }
}

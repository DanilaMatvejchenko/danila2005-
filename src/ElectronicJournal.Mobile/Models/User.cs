namespace ElectronicJournal.Mobile.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // Student, Teacher, Admin
    public string? AvatarUrl { get; set; }
    public string? Phone { get; set; }
    public int? GroupId { get; set; }
    public string? GroupName { get; set; }

    public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
    public string ShortName => $"{LastName} {(FirstName.Length > 0 ? FirstName[0] + "." : "")} {(MiddleName.Length > 0 ? MiddleName[0] + "." : "")}".Trim();
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public User User { get; set; } = new();
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

namespace ElectronicJournal.Web.Models;

public class AuthResponse
{
    public string Token { get; set; } = "";
    public string UserName { get; set; } = "";
    public string Role { get; set; } = "";
    public int UserId { get; set; }
}

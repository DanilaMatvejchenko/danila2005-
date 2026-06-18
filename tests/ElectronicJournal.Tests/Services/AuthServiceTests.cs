using ElectronicJournal.API.Data;
using ElectronicJournal.API.DTOs;
using ElectronicJournal.API.Models;
using ElectronicJournal.API.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace ElectronicJournal.Tests.Services;

public class AuthServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "TestSecretKeyThatIsLongEnoughForHmacSha256Algorithm123!",
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience",
                ["Jwt:ExpireHours"] = "24"
            })
            .Build();

        var logger = new Mock<ILogger<AuthService>>();
        _service = new AuthService(_context, config, logger.Object);
    }

    [Fact]
    public async Task Register_CreatesNewUser()
    {
        var request = new RegisterRequest
        {
            Email = "new@test.ru",
            Password = "Password123!",
            FirstName = "Тест",
            LastName = "Тестов",
            Role = UserRole.Student
        };

        var result = await _service.RegisterAsync(request);

        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
        result.Email.Should().Be("new@test.ru");
    }

    [Fact]
    public async Task Register_ReturnNull_WhenEmailExists()
    {
        _context.Users.Add(new User
        {
            Email = "existing@test.ru", PasswordHash = "hash",
            FirstName = "A", LastName = "B", Role = UserRole.Student
        });
        await _context.SaveChangesAsync();

        var request = new RegisterRequest
        {
            Email = "existing@test.ru",
            Password = "Pass123!",
            FirstName = "C", LastName = "D",
            Role = UserRole.Student
        };

        var result = await _service.RegisterAsync(request);
        result.Should().BeNull();
    }

    [Fact]
    public async Task Login_ReturnsToken_WhenCredentialsValid()
    {
        await _service.RegisterAsync(new RegisterRequest
        {
            Email = "login@test.ru", Password = "Password123!",
            FirstName = "A", LastName = "B", Role = UserRole.Student
        });

        var result = await _service.LoginAsync(new LoginRequest
        {
            Email = "login@test.ru", Password = "Password123!"
        });

        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_ReturnsNull_WhenPasswordWrong()
    {
        await _service.RegisterAsync(new RegisterRequest
        {
            Email = "wrong@test.ru", Password = "Password123!",
            FirstName = "A", LastName = "B", Role = UserRole.Student
        });

        var result = await _service.LoginAsync(new LoginRequest
        {
            Email = "wrong@test.ru", Password = "WrongPassword!"
        });

        result.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

using CommunityToolkit.Maui;
using ElectronicJournal.Mobile.Services;
using ElectronicJournal.Mobile.ViewModels;
using ElectronicJournal.Mobile.Views;
using Microsoft.Extensions.Logging;

namespace ElectronicJournal.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // HttpClient — change URL for your environment:
        // Android emulator: https://10.0.2.2:5001/
        // Physical device: use your server's IP
        // Windows/iOS simulator: https://localhost:5001/
        var baseUrl = DeviceInfo.Platform == DevicePlatform.Android
            ? "https://10.0.2.2:5001/"
            : "https://localhost:5001/";
        builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(baseUrl) });

        // Services
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IApiService, ApiService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();

        // ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<JournalViewModel>();
        builder.Services.AddTransient<ScheduleViewModel>();
        builder.Services.AddTransient<AttendanceViewModel>();
        builder.Services.AddTransient<ChatViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();

        // Views
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<JournalPage>();
        builder.Services.AddTransient<SchedulePage>();
        builder.Services.AddTransient<AttendancePage>();
        builder.Services.AddTransient<ChatPage>();
        builder.Services.AddTransient<ProfilePage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

namespace ElectronicJournal.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("login", typeof(Views.LoginPage));
        Routing.RegisterRoute("dashboard", typeof(Views.DashboardPage));
        Routing.RegisterRoute("journal", typeof(Views.JournalPage));
        Routing.RegisterRoute("schedule", typeof(Views.SchedulePage));
        Routing.RegisterRoute("attendance", typeof(Views.AttendancePage));
        Routing.RegisterRoute("chat", typeof(Views.ChatPage));
        Routing.RegisterRoute("profile", typeof(Views.ProfilePage));
    }
}

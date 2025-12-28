using YuvaCep.Mobile.Views;

namespace YuvaCep.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("RegisterPage", typeof(Views.RegisterPage));
        Routing.RegisterRoute("CreateClassPage", typeof(CreateClassPage));
        Routing.RegisterRoute("ParentHomePage", typeof(ParentHomePage));
        Routing.RegisterRoute("TeacherHomePage", typeof(TeacherHomePage));
        Routing.RegisterRoute("DailyReportPage", typeof(DailyReportPage));
        Routing.RegisterRoute("AnnouncementsPage", typeof(AnnouncementsPage));
        Routing.RegisterRoute("StudentListPage", typeof(StudentListPage));
        Routing.RegisterRoute("MealPlanPage", typeof(Views.MealPlanPage));
        Routing.RegisterRoute("StudentDetailPage", typeof(Views.StudentDetailPage));
        //Routing.RegisterRoute("LoginPage", typeof(Views.LoginPage));
        Routing.RegisterRoute("ParentDailyReportPage", typeof(Views.ParentDailyReportPage));
        Routing.RegisterRoute("BadgeDetailPage", typeof(Views.BadgeDetailPage));
        Routing.RegisterRoute("BadgeTrackingPage", typeof(Views.BadgeTrackingPage));
        Routing.RegisterRoute("LoginPage", typeof(YuvaCep.Mobile.Views.LoginPage));
        Routing.RegisterRoute(nameof(StudentDetailPage), typeof(StudentDetailPage));
    }
}


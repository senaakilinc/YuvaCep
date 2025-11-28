using YuvaCep.Mobile.Views;

namespace YuvaCep.Mobile;

public partial class AppShell : Shell
    {
    public AppShell()
    {
        InitializeComponent();

        // RegisterPage rotasını buraya kaydediyoruz
        Routing.RegisterRoute("RegisterPage", typeof(Views.RegisterPage));
        Routing.RegisterRoute("CreateClassPage", typeof(CreateClassPage));
        Routing.RegisterRoute("ParentHomePage", typeof(ParentHomePage));
        Routing.RegisterRoute("TeacherHomePage", typeof(TeacherHomePage));
        Routing.RegisterRoute("DailyReportPage", typeof(DailyReportPage));
        Routing.RegisterRoute("AnnouncementsPage", typeof(AnnouncementsPage));
    }
}


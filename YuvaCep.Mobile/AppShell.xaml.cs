using YuvaCep.Mobile.Views;

namespace YuvaCep.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // --- GİRİŞ VE KAYIT ---
        Routing.RegisterRoute("Login_Route", typeof(LoginPage));
        Routing.RegisterRoute("Register_Route", typeof(RegisterPage));
        Routing.RegisterRoute("RoleSelection_Route", typeof(RoleSelectionPage));

        // --- ÖĞRETMEN ROTALARI ---
        Routing.RegisterRoute("CreateClass_Route", typeof(CreateClassPage));
        Routing.RegisterRoute("TeacherDailyReport_Route", typeof(TeacherDailyReportPage));
        Routing.RegisterRoute("StudentList_Route", typeof(StudentListPage));

        // --- DETAY VE DİĞER ROTALAR ---
        Routing.RegisterRoute("StudentDetail_Route", typeof(StudentDetailPage));
        Routing.RegisterRoute("ParentHome_Route", typeof(ParentHomePage));
        Routing.RegisterRoute("DailyReport_Route", typeof(DailyReportPage));
        Routing.RegisterRoute("Announcements_Route", typeof(AnnouncementsPage));
        Routing.RegisterRoute("BadgeDetail_Route", typeof(BadgeDetailPage));
        Routing.RegisterRoute("BadgeTracking_Route", typeof(BadgeTrackingPage));
        Routing.RegisterRoute("StudentCards_Route", typeof(StudentCardsPage));
        Routing.RegisterRoute("FoodList_Route", typeof(FoodListPage));
        Routing.RegisterRoute("CurriculumPage", typeof(CurriculumPage));


    }
}
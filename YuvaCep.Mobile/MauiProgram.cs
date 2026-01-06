using Microsoft.Extensions.Logging;
using YuvaCep.Mobile.Services;
using YuvaCep.Mobile.ViewModels;
using YuvaCep.Mobile.Views;
using System.Globalization;
using CommunityToolkit.Maui;

namespace YuvaCep.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var culture = new CultureInfo("tr-TR");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // --- SERVİSLER ---
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<AnnouncementService>();
            builder.Services.AddSingleton<DailyReportService>();
            builder.Services.AddSingleton<StudentService>();
            builder.Services.AddSingleton<ClassService>();
            builder.Services.AddSingleton<FoodListService>();
            builder.Services.AddSingleton<CurriculumService>();
            builder.Services.AddSingleton<ActivityService>();


            // --- GİRİŞ & KAYIT EKRANLARI ---
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginViewModel>();

            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<RegisterViewModel>();

            builder.Services.AddTransient<RoleSelectionPage>();
            builder.Services.AddTransient<RoleSelectionViewModel>();

            // --- 3. VELİ EKRANLARI ---
            builder.Services.AddTransient<ParentHomePage>();
            builder.Services.AddTransient<ParentHomeViewModel>();

            builder.Services.AddTransient<StudentCardsPage>();
            builder.Services.AddTransient<StudentCardsViewModel>();

            builder.Services.AddTransient<AnnouncementsPage>();
            builder.Services.AddTransient<AnnouncementsViewModel>();

            builder.Services.AddTransient<DailyReportPage>();
            builder.Services.AddTransient<DailyReportViewModel>();

            builder.Services.AddTransient<StudentChartsListViewModel>();
            builder.Services.AddTransient<StudentChartsListPage>();

            builder.Services.AddTransient<StudentChartDetailViewModel>();
            builder.Services.AddTransient<StudentChartDetailPage>();

            // --- ÖĞRETMEN EKRANLARI ---

            builder.Services.AddTransient<CreateClassPage>();
            builder.Services.AddTransient<CreateClassViewModel>();

            builder.Services.AddTransient<TeacherHomePage>();
            builder.Services.AddTransient<TeacherHomeViewModel>();

            builder.Services.AddTransient<TeacherDailyReportPage>();
            builder.Services.AddTransient<TeacherDailyReportViewModel>();

            builder.Services.AddTransient<StudentListPage>();
            builder.Services.AddTransient<StudentListViewModel>();

            builder.Services.AddTransient<StudentDetailPage>();
            builder.Services.AddTransient<StudentDetailViewModel>();

            builder.Services.AddTransient<FoodListPage>();
            builder.Services.AddTransient<FoodListViewModel>();

            builder.Services.AddTransient<CurriculumViewModel>();
            builder.Services.AddTransient<CurriculumPage>();

            builder.Services.AddTransient<CreateActivityViewModel>();
            builder.Services.AddTransient<CreateActivityPage>();

            builder.Services.AddTransient<ActivityChartsListViewModel>();
            builder.Services.AddTransient<ActivityChartsListPage>();

            builder.Services.AddTransient<ActivityChartDetailViewModel>();
            builder.Services.AddTransient<ActivityChartDetailPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif       
            return builder.Build();
        }
    }
}
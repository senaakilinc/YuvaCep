using Microsoft.Extensions.Logging;
using YuvaCep.Mobile.Services;   
using YuvaCep.Mobile.ViewModels; 
using YuvaCep.Mobile.Views;
using System.Globalization;

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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddTransient<YuvaCep.Mobile.Views.LoginPage>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddTransient<ParentHomePage>();
            builder.Services.AddTransient<ParentHomeViewModel>();
            builder.Services.AddTransient<StudentDetailPage>();
            builder.Services.AddTransient<StudentDetailViewModel>();
            builder.Services.AddSingleton<AnnouncementService>();
            builder.Services.AddTransient<AnnouncementsPage>();
            builder.Services.AddTransient<AnnouncementsViewModel>();
            builder.Services.AddSingleton<DailyReportService>();
            builder.Services.AddTransient<DailyReportPage>();
            builder.Services.AddTransient<DailyReportViewModel>();
            builder.Services.AddTransient<YuvaCep.Mobile.Views.TeacherDailyReportPage>();
            builder.Services.AddTransient<YuvaCep.Mobile.ViewModels.TeacherDailyReportViewModel>();
            builder.Services.AddSingleton<StudentService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif      
            return builder.Build();
        }
    }
}

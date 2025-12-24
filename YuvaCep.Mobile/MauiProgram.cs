using Microsoft.Extensions.Logging;
using YuvaCep.Mobile.Services;   
using YuvaCep.Mobile.ViewModels; 
using YuvaCep.Mobile.Views;
namespace YuvaCep.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginViewModel>();


#if DEBUG
            builder.Logging.AddDebug();
#endif      
            return builder.Build();
        }
    }
}

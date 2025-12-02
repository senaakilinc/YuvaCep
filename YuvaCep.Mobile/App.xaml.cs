using Microsoft.Extensions.DependencyInjection;
using YuvaCep.Mobile;

namespace YuvaCep.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            UserAppTheme = AppTheme.Light;

            MainPage = new AppShell();

            // --- BENİ HATIRLA KONTROLÜ ---
            CheckLoginStatus();
        }
        private async void CheckLoginStatus()
        {
            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);
            string userRole = Preferences.Get("UserRole", "");

            if (isLoggedIn)
            {
                // Kullanıcı daha önce "Beni Hatırla" demiş.
                // Direkt ilgili ana sayfaya yönlendir.
                if (userRole == "Teacher")
                {
                    await Shell.Current.GoToAsync("TeacherHomePage");
                }
                else if (userRole == "Parent")
                {
                    await Shell.Current.GoToAsync("ParentHomePage");
                }
            }
            else
            {
                // Giriş yapılmamışsa Rol Seçim Ekranına git
                await Shell.Current.GoToAsync("//RoleSelectionPage");
            }
        }

    }
}
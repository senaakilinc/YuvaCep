using Microsoft.Extensions.DependencyInjection;
using YuvaCep.Mobile;

namespace YuvaCep.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            UserAppTheme = AppTheme.Light;
        }

        protected override async void OnStart()
        {
            base.OnStart();
            await CheckLoginStatus();
        }

        private async Task CheckLoginStatus()
        {

            await Task.Delay(500);

            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);
            string userRole = Preferences.Get("UserRole", "");

            if (isLoggedIn)
            {

                if (userRole == "Teacher")
                {
                    await Shell.Current.GoToAsync("//TeacherHomePage");
                }
                else
                {
                    await Shell.Current.GoToAsync("//ParentHomePage");
                }
            }

        }
    }
}
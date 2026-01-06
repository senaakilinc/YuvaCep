using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace YuvaCep.Mobile.ViewModels
{
    public class dashboardItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string ColorHex { get; set; }
        public string Route { get; set; }
    }

    public partial class TeacherHomeViewModel : ObservableObject
    {
        [ObservableProperty]
        private string className;

        [ObservableProperty]
        private string teacherName;

        [ObservableProperty]
        private string greetingMessage;

        public ObservableCollection<dashboardItem> MenuItems { get; } = new();

        public TeacherHomeViewModel()
        {
            LoadData();

            MenuItems.Add(new dashboardItem { Title = "Günlük Rapor", Icon = "📝", ColorHex = "#60A5FA", Route = "TeacherDailyReport_Route" });
            MenuItems.Add(new dashboardItem { Title = "Duyuru Yayınla", Icon = "📢", ColorHex = "F472B6", Route = "Announcements_Route" });
            MenuItems.Add(new dashboardItem { Title = "Yemek Listesi", Icon = "🍎", ColorHex = "#34D399", Route = "FoodList_Route" });
            MenuItems.Add(new dashboardItem { Title = "Ders Programı", Icon = "📅", ColorHex = "#FBBF24", Route = "CurriculumPage" });
            MenuItems.Add(new dashboardItem { Title = "Öğrenci Listesi", Icon = "👶", ColorHex = "#A78BFA", Route = "StudentList_Route" });
            MenuItems.Add(new dashboardItem { Title = "Rozet Takibi", Icon = "🏆", ColorHex = "#F59E0B", Route = "BadgeTracking_Route" });
            MenuItems.Add(new dashboardItem { Title = "Aylık Çizelgeler", Icon = "🧩", ColorHex = "#8B5CF6", Route = "ActivityChartsListPage" });
        }

        public void LoadData()
        {
            TeacherName = Preferences.Get("UserName", "Değerli Öğretmenimiz");
            ClassName = Preferences.Get("ClassName", "Sınıf Bilgisi Yok");

            var hour = DateTime.Now.Hour;

            if (hour >= 6 && hour < 12)
            {
                GreetingMessage = "Günaydın! ☀️";
            }
            else if (hour >= 12 && hour < 18)
            {
                GreetingMessage = "İyi Günler! 👋";
            }
            else
            {
                GreetingMessage = "İyi Akşamlar! 🌙";
            }
        }

        [RelayCommand]
        private async Task NavigateAsync(string route)
        {
            if (string.IsNullOrEmpty(route))
            {
                await Shell.Current.DisplayAlert("Bilgi", "Bu özellik yakında eklenecek.", "Tamam");
                return;
            }

            try
            {
                await Shell.Current.GoToAsync(route);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", "Bu sayfa henüz hazır değil veya rota bulunamadı.", "Tamam");
            }
        }

        [RelayCommand]
        private async Task GoToFoodListAsync()
        {
            await Shell.Current.GoToAsync("FoodList_Route");
        }

        [RelayCommand]
        private async Task LogoutAsync()
        {
            bool answer = await Shell.Current.DisplayAlert("Çıkış", "Hesabınızdan çıkış yapmak istiyor musunuz?", "Evet", "Hayır");

            if (answer)
            {
                SecureStorage.Remove("auth_token");
                Preferences.Clear();
                Application.Current.MainPage = new AppShell();
            }
        }
    }
}
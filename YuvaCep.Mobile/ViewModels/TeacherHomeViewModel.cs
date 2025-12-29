using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Mobile.ViewModels
{
    // Ekranda göstereceğimiz her bir kartın modeli
    public class dashboardItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string ColorHex { get; set; } //Kartın rengi
        public string Route { get; set; }  //Tıklayınca gideceğimiz ekran

    }

    public partial class TeacherHomeViewModel : ObservableObject
    {
        [ObservableProperty]
        private string className = "Papatyalar Sınıfı"; //API'den gelecek

        [ObservableProperty]
        private string teacherName = "Ayşe Yılmaz";

        //Ekranda gözükecek kartların listesi
        public ObservableCollection<dashboardItem> MenuItems { get; } = new();

        public TeacherHomeViewModel()
        {
            //Menüleri dahil ediyoruz
            MenuItems.Add(new dashboardItem { Title = "Günlük Rapor", Icon = "📝", ColorHex = "#60A5FA", Route = "TeacherDailyReportPage" });
            MenuItems.Add(new dashboardItem { Title = "Duyuru Yayınla", Icon = "📢", ColorHex = "F472B6", Route = "AnnouncementsPage" });
            MenuItems.Add(new dashboardItem { Title = "Yemek Listesi", Icon = "🍎", ColorHex = "#34D399", Route = "MealPlanPage" });
            MenuItems.Add(new dashboardItem { Title = "Ders Programı", Icon = "📅", ColorHex = "#FBBF24", Route = "CurriculumPage" });
            MenuItems.Add(new dashboardItem { Title = "Öğrenci Listesi", Icon = "👶", ColorHex = "#A78BFA", Route = "StudentListPage" });
            MenuItems.Add(new dashboardItem { Title = "Rozet Takibi", Icon = "🏆", ColorHex = "#F59E0B", Route = "BadgeTrackingPage" });
        }

        [RelayCommand]
        private async Task NavigateAsync(string route)
        {
            if (string.IsNullOrEmpty(route))
            {
                await Shell.Current.DisplayAlert("Bilgi", "Bu özellik yakında eklenecek.", "Tamam");
                return;
            }

            await Shell.Current.GoToAsync(route);
        }

        [RelayCommand]
        private async Task LogoutAsync()
        {
            bool answer = await Shell.Current.DisplayAlert("Çıkış", "Hesabınızdan çıkış yapmak istiyor musunuz?", "Evet", "Hayır");

            if (answer)
            {
                // 1. Kayıtlı bilgileri sil (Beni Hatırla'yı iptal et)
                Preferences.Clear();

                // 2. En başa (Rol Seçimi) yönlendir
                await Shell.Current.GoToAsync("//RoleSelectionPage");
            }
        }
    }

}

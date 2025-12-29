using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using YuvaCep.Mobile.Dtos;

namespace YuvaCep.Mobile.ViewModels
{
    // Sayfaya parametre olarak gelen öğrenci bilgisini alacağız
    [QueryProperty(nameof(Student), "Student")]
    [QueryProperty(nameof(ClassName), "className")]
    public partial class StudentDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private StudentDto student;

        [ObservableProperty]
        private string className;

        // --- ROL KONTROLÜ ---
        public bool IsTeacher => Preferences.Get("UserRole", "") == "Teacher";

        // --- ÖĞRETMEN İÇİN RAPOR EKLEME ---
        [RelayCommand]
        private async Task GoToTeacherDailyReportAsync()
        {
            if (Student == null) return;

            var navigationParameter = new Dictionary<string, object>
            {
                { "Student", Student } // Öğrenci bilgisini rapor ekleme sayfasına taşıyoruz
            };

            // Öğretmen rapor sayfasına yönlendiriyoruz
            await Shell.Current.GoToAsync("TeacherDailyReportPage", navigationParameter);
        }

        // 1. Günlük Raporu Gör (VELİ İÇİN)
        [RelayCommand]
        private async Task GoToDailyReportAsync()
        {
            if (Student == null) return;
            var navigationParameter = new Dictionary<string, object>
            {
                { "Student", Student }
            };

            await Shell.Current.GoToAsync("DailyReportPage", navigationParameter);
        }

        // 2. Yemek Listesini Gör
        [RelayCommand]
        private async Task GoToFoodListAsync()
        {
            await Shell.Current.DisplayAlert("Bilgi", "Yemek Listesi Sayfası Hazırlanıyor...", "Tamam");
        }

        [RelayCommand]
        private async Task GoToLessonProgramAsync()
        {
            await Shell.Current.DisplayAlert("Bilgi", "Ders Programı Sayfası Hazırlanıyor...", "Tamam");
        }

        // 3. Duyuruları Gör
        [RelayCommand]
        private async Task GoToAnnouncementsAsync()
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "Student", Student }
            };

            await Shell.Current.GoToAsync("AnnouncementsPage", navigationParameter);
        }

        // 4. Rozet Detaylarını Gör
        [RelayCommand]
        private async Task GoToBadgeDetailsAsync()
        {
            await Shell.Current.GoToAsync("BadgeDetailPage");
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
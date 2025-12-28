using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using YuvaCep.Mobile.Dtos;

namespace YuvaCep.Mobile.ViewModels
{
    //Sayfaya parametre olarak gelen öğrenci bilgisini alacağız
    [QueryProperty(nameof(Student), "Student")]
    [QueryProperty(nameof(ClassName), "className")]
    public partial class StudentDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private StudentDto student;

        [ObservableProperty]
        private string className;

        //1. Günlük Raporu Gör
        [RelayCommand]
        private async Task GoToDailyReportAsync()
        {
            await Shell.Current.GoToAsync("ParentDailyReportPage");
        }

        //2. Yemek Listesini Gör
        [RelayCommand]
        private async Task GoToFoodListAsync()
        {
            //Öğretmenin yüklediği yemek listesinin görüneceği sayfa
            await Shell.Current.DisplayAlert("Bilgi", "Yemek Listesi Sayfası Hazırlanıyor...", "Tamam");
            //İleride -> await Shell.Current.GoToAsync("ParentMealPlanPage");
        }

        [RelayCommand]
        private async Task GoToLessonProgramAsync()
        {
            await Shell.Current.DisplayAlert("Bilgi", "Ders Programı Sayfası Hazırlanıyor...", "Tamam");
        }

        //3. Duyuruları Gör
        [RelayCommand]
        private async Task GoToAnnouncementsAsync()
        {
            await Shell.Current.DisplayAlert("Bilgi", "Duyurular Sayfası Hazırlanıyor...", "Tamam");
            // İleride: await Shell.Current.GoToAsync("ParentAnnouncementsPage");
        }

        //4.Rozet Detaylarını Gör
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

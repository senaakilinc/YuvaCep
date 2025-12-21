using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace YuvaCep.Mobile.ViewModels
{
    //Sayfaya parametre olarak gelen öğrenci bilgisini alacağız
    [QueryProperty(nameof(StudentName), "name")]
    [QueryProperty(nameof(ClassName), "className")]
    public partial class StudentDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private string studentName = "Yükleniyor...";

        [ObservableProperty]
        private string className = "Papatyalar Sınıfı"; //Örnek sınıf

        //1. Günlük Raporu Gör
        [RelayCommand]
        private async Task GoToReportAsync()
        {
            await Shell.Current.GoToAsync("ParentDailyReportPage");
        }

        //2. Yemek Listesini Gör
        [RelayCommand]
        private async Task GoToMealPlanAsync()
        {
            //Öğretmenin yüklediği yemek listesinin görüneceği sayfa
            await Shell.Current.DisplayAlert("Bilgi", "Yemek Listesi Sayfası Hazırlanıyor...", "Tamam");
            //İleride -> await Shell.Current.GoToAsync("ParentMealPlanPage");
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

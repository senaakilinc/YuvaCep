using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace YuvaCep.Mobile.ViewModels
{

    public class ChildCardItem
    {
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string PhotoUrl { get; set; }

    }
    public partial class ParentHomeViewModel : ObservableObject
    {
        //Ekranda listelenen çocuklar.
        public ObservableCollection<ChildCardItem> MyChild { get; } = new()
        {
            new ChildCardItem
            {
                Name = "Ali Yılmaz",
                ClassName = "Papatyalar Sınıfı",
                PhotoUrl = "child_avatar.png"
            }
        };
        public ParentHomeViewModel()
        {
         
        }

        [RelayCommand]
        private async Task AddChildByCodeAsync()
        {
            // Kullanıcıdan pop-up ile referans kodu iste.
            string code = await Shell.Current.DisplayPromptAsync("Çocuk Ekle", "Öğretmenden alınan 6 Haneli Referans Kodunu Giriniz: ", "Ekle", "İptal", "Örn: E09N30", 6);

            // İptal edildiyse veya boş bırakılırsa dur.
            if (string.IsNullOrWhiteSpace(code)) return;

            // Kodu API a gönderip eşleşen cevap alınırsa:
            // var child = await _apiService.GetChildByCode(code);

            //Yeni çocuk ekleme için DEMO
            await Shell.Current.DisplayAlert("Başarılı", $"{code} kodu doğrulandı!", "Tamam");

            // Listeye ekle (Örnek veri)
            MyChild.Add(new ChildCardItem
            {
                Name = "Emin Badur",
                ClassName = "Papatyalar Sınıfı",
                PhotoUrl = "child_avatar.png"
            });
        }

        [RelayCommand]
        private async Task OpenChildReportAsync(ChildCardItem child)
        {
            if(child == null) return;
            // İleride çocuğun detayına/günlük raporuna gitmek için
            await Shell.Current.GoToAsync($"StudentDetailPage?name = {child.Name}");
        }


        [RelayCommand]
        private async Task LogoutAsync()
        {
            bool answer = await Shell.Current.DisplayAlert("Çıkış", "Hesabınızdan çıkış yapmak istiyor musunuz?", "Evet", "Hayır");

            if (answer)
            {
                // 1. Oturumu temizle
                Preferences.Clear();

                // 2. En başa (Rol Seçimi) dön
                await Shell.Current.GoToAsync("//RoleSelectionPage");
            }
        }

    }
}

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

    }
    public partial class ParentHomeViewModel : ObservableObject
    {
        //Ekranda listelenen çocuklar.
        public ObservableCollection<ChildCardItem> MyChild { get; } = new();

        [RelayCommand]
        private async Task AddChildByCodeAsync()
        {
            // Kullanıcıdan pop-up ile referans kodu iste.
            string code = await Shell.Current.DisplayPromptAsync("Çocuk Ekle", "Öğretmenden alınan 6 Haneli Referans Kodunu Giriniz: ", "Ekle", "İptal", "Örn: E09N30", 6);

            // İptal edildiyse veya boş bırakılırsa dur.
            if (string.IsNullOrWhiteSpace(code)) return;

            // Kodu API a gönderip eşleşen cevap alınırsa:
            // var child = await _apiService.GetChildByCode(code);

            await Shell.Current.DisplayAlert("Başarılı", $"{code} kodu doğrulandı!", "Tamam");

            // Listeye ekle (Örnek veri)
            MyChild.Add(new ChildCardItem
            {
                Name = "Emin Badur",
                ClassName = "Papatyalar Sınıfı"
            });
        }

        [RelayCommand]
        private async Task OpenChildReportAsync(ChildCardItem child)
        {
            // İleride çocuğun detayına/günlük raporuna gitmek için
            await Shell.Current.DisplayAlert("Rapor", $"{child.Name} için raporlar açılıyor...", "Tamam");
        }

    }
}

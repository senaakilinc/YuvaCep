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

    //Ekranda eski duyuruları da listelemek için model
    public class AnnouncementItem
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }

    }

    public partial class AnnouncementsViewModel : ObservableObject
    {
        //Yeni duyuru alanları
        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string content;

        //Geçmiş duyurular listesi (Sayfanın altında görünür)
        public ObservableCollection<AnnouncementItem> History { get; } = new();

        public AnnouncementsViewModel()
        {
            //Örnek veriler
            History.Add(new AnnouncementItem { Title = "Hoşgeldiniz", Content = "Yeni eğitim yılımız hayırlı olsun.", Date = "01.09.2025" });
        }

        [RelayCommand]
        private async Task SendAnnouncementAsync()
        {
            if ((string.IsNullOrWhiteSpace(title)) || (string.IsNullOrWhiteSpace(content)))
            {
                await Shell.Current.DisplayAlert("Eksik Bilgi!", "Lütfen başlık ve içerik giriniz.", "Tamam");
                return;
            }

            //2. Onay penceresi (EVET / HAYIR)
            bool answer = await Shell.Current.DisplayAlert(
                "Yayınla",
                "Bu duyuruyu yayınlamak istiyor musunuz? (Tüm velilere bildirim gönderilecektir!)",
                "Evet",
                "Hayır");


            //Eğer kullanıcı HAYIR'a bastıysa işlemden çık
            if (!answer) return;

            //3. Listeye ekle (Kullanıcı EVET'e basarsa burası çalışır)
            History.Insert(0, new AnnouncementItem
            {
                Title = Title,
                Content = content,
                Date = DateTime.Now.ToString("dd.MM.yyyy")
            });

            //4. Başarılı mesajı
            await Shell.Current.DisplayAlert("Başarılı", "Duyuru başarıyla yayımlandı.", "Tamam");

            //5. Kutucukları temizle
            Title = string.Empty;
            Content = string.Empty;

        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}

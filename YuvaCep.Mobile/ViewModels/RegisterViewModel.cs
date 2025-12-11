using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace YuvaCep.Mobile.ViewModels
{
    public partial class RegisterViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        private string userRole; // "Parent" veya "Teacher"

        [ObservableProperty]
        private string pageTitle; // Başlık (Öğretmen/Veli)


        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private string surname;
        [ObservableProperty]
        private string tcIdNumber;
        [ObservableProperty]
        private string phoneNumber;
        [ObservableProperty]
        private string password;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("role"))
            {
                UserRole = query["role"].ToString();

                // Başlığı güncelle
                if (UserRole == "Teacher")
                    PageTitle = "Öğretmen Kaydı";
                else
                    PageTitle = "Veli Kaydı";
            }
        }

        [RelayCommand]
        private async Task RegisterAsync()
        {
            //Buraya ileride API'ye kayıt olma kodu gelecek.

            //Basit bir doğrulama 
            if ((string.IsNullOrWhiteSpace(name)) || string.IsNullOrWhiteSpace(tcIdNumber))
            {
                await Shell.Current.DisplayAlert("Hata", "Lütfen tüm alanları doldurun.", "Tamam");
                return;
            }

            await Shell.Current.DisplayAlert("Başarılı", "Kayıt Oluşturuldu.", "Tamam");

            // --- BENİ HATIRLA (Otomatik Giriş) ---
            // Kayıt olunca otomatik giriş yapmış sayalım
            Preferences.Set("IsLoggedIn", true);
            Preferences.Set("UserRole", userRole);

            // --- YÖNLENDİRME ---
            if (userRole == "Teacher")
            {
                // Öğretmen -> Sınıf Oluşturmaya gider
                await Shell.Current.GoToAsync("CreateClassPage");
            }
            else
            {
                // Veli -> Çocuklarım sayfasına gider
                await Shell.Current.GoToAsync("ParentHomePage");
            }

        }
        [RelayCommand]
        private async Task GoBackAsync()
        {
            //Geri giriş sayfasına dön.
            await Shell.Current.GoToAsync("...");
        }

    }
}

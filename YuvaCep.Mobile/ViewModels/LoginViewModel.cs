using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace YuvaCep.Mobile.ViewModels
{   
    public partial class LoginViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        private string userRole; // "Parent" veya "Teacher"

        [ObservableProperty]
        private string tcKimlikNo;
        
        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private bool isRememberMeChecked; // Beni Hatırla Checkbox'ı


        [ObservableProperty]
        private string registerButtonText;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("role"))
            {
                // Gelen veriyi al (Teacher veya Parent)
                UserRole = query["role"].ToString();

                // Buton yazısını hemen güncelle
                if (UserRole == "Teacher")
                    RegisterButtonText = "Kayıt Ol (Öğretmen)";
                else
                    RegisterButtonText = "Kayıt Ol (Veli)";
            }
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            // Ekrana uyarı verirken de TC'yi gösterelim.
            if (string.IsNullOrWhiteSpace(tcKimlikNo) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Hata", "Lütfen tüm alanları doldurunuz", "Tamam");
                return;
            }

            // --- BENİ HATIRLA ---
            if (isRememberMeChecked)
            {
                // Bilgileri telefona kaydediyoruz
                Preferences.Set("IsLoggedIn", true);
                Preferences.Set("UserRole", userRole);
                Preferences.Set("UserName", tcKimlikNo);
            }

            // --- YÖNLENDİRME ---
            if (userRole == "Teacher")
            {
                // Öğretmen -> TeacherHomePage
                await Shell.Current.GoToAsync("TeacherHomePage");
            }
            else
            {
                // Veli -> ParentHomePage
                await Shell.Current.GoToAsync("ParentHomePage");
            }
        }
        [RelayCommand]
        private async Task GoToRegisterAsync()
        {
        // Kayıt sayfasına da rol bilgisini gönderiyoruz
        await Shell.Current.GoToAsync($"RegisterPage?role={userRole}");
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
        await Shell.Current.GoToAsync("..");
        }

    }
 
}

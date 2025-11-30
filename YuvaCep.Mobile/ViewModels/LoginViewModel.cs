using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace YuvaCep.Mobile.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string tcKimlikNo;
        [ObservableProperty]
        private string password;
        [RelayCommand]
        private async Task LoginAsync()
        {
            // Ekrana uyarı verirken de TC'yi gösterelim.
            if (string.IsNullOrWhiteSpace(tcKimlikNo) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Hata", "Lütfen tüm alanları doldurunuz", "Tamam");
                return;
            }
            // Buraya ileride gerçek API kontrolü gelecek
            await Shell.Current.DisplayAlert("Bilgi", $"Giriş Yapılıyor: {tcKimlikNo}", "Tamam");

            // Başarılı olursa ana sayfaya yönlendirme:
            // await Shell.Current.GoToAsync("//MainPage");
            await Shell.Current.GoToAsync("ParentHomePage");
        }

        [RelayCommand]
        private async Task GoToRegisterAsync()
        {
            // Kayıt sayfasına yönlendirme
            await Shell.Current.GoToAsync("RegisterPage");
        }

    }
 
}

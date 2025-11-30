using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace YuvaCep.Mobile.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
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

            await Shell.Current.DisplayAlert("Başarılı", $"{name} Öğretmeni kaydediliyor...", "Tamam");

            await Shell.Current.GoToAsync("CreateClassPage");

        }
        [RelayCommand]
        private async Task GoBackAsync()
        {
            //Geri giriş sayfasına dön.
            await Shell.Current.GoToAsync("...");
        }

    }
}

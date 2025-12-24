using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YuvaCep.Mobile.Services;

namespace YuvaCep.Mobile.ViewModels
{   
    public partial class LoginViewModel : ObservableObject, IQueryAttributable
    {
        private readonly AuthService _authService;

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
        }

        [ObservableProperty]
        private string userRole = string.Empty; // "Parent" veya "Teacher"

        [ObservableProperty]
        private string tcKimlikNo = string.Empty;
        
        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private bool isRememberMeChecked; // Beni Hatırla Checkbox'ı


        [ObservableProperty]
        private string registerButtonText = "Kayıt Ol";

        [ObservableProperty]
        private bool isBusy; // İşlem sırasında butonu kilitlemek için

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
            if (IsBusy) return;

            // Ekrana uyarı verirken de TC'yi gösterelim.
            if (string.IsNullOrWhiteSpace(TcKimlikNo) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Hata", "Lütfen tüm alanları doldurunuz", "Tamam");
                return;
            }

            try
            {
                IsBusy = true;

                var response = await _authService.LoginAsync(TcKimlikNo, Password);

                if (response != null && response.IsSuccess)
                {
                    Preferences.Set("AuthToken", response.Token);

                    string roleFromServer = response.UserRole;
                    Preferences.Set("UserRole", roleFromServer);

                    if (IsRememberMeChecked)
                    {
                        Preferences.Set("IsLoggedIn", true);
                    }

                    await Shell.Current.DisplayAlert("Başarılı", $"Hoşgeldiniz, {response.Message}", "Tamam");

                    if (roleFromServer == "Teacher")
                    {
                        await Shell.Current.GoToAsync("//TeacherHomePage");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync("//ParentHomePage");
                    }
                }
                else
                {
                    string errorMsg = response?.Message ?? "Giriş Başarısız. Bilgileri kontrol edin.";
                    await Shell.Current.DisplayAlert("Hata", errorMsg, "Tamam");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Bağlantı hatası: {ex.Message}", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToRegisterAsync()
        {
            // Kayıt sayfasına rol bilgisiyle git
            await Shell.Current.GoToAsync($"RegisterPage?role={UserRole}");
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

    }
 
}

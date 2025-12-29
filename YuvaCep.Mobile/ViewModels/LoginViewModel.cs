using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        private string userRole = string.Empty;

        [ObservableProperty]
        private string tcKimlikNo = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private bool isRememberMeChecked;

        [ObservableProperty]
        private string registerButtonText = "Kayıt Ol";

        [ObservableProperty]
        private bool isBusy;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("role"))
            {
                UserRole = query["role"].ToString();
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
                    // Token ve Rolü Kaydet
                    Preferences.Set("AuthToken", response.Token);
                    string roleFromServer = response.UserRole;
                    Preferences.Set("UserRole", roleFromServer);

                    // İsmi Kaydet
                    string fullName = $"{response.Name} {response.Surname}";
                    Preferences.Set("UserName", fullName);

                    if (IsRememberMeChecked)
                    {
                        Preferences.Set("IsLoggedIn", true);
                    }

                    // Hoşgeldiniz mesajında ismi gösterelim
                    await Shell.Current.DisplayAlert("Başarılı", $"Hoşgeldiniz, {fullName}", "Tamam");

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
            await Shell.Current.GoToAsync($"RegisterPage?role={UserRole}");
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
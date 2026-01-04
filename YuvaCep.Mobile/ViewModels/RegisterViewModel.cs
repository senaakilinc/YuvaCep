using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using YuvaCep.Mobile.Dtos;
using YuvaCep.Mobile.Services;

namespace YuvaCep.Mobile.ViewModels
{
    public partial class RegisterViewModel : ObservableObject, IQueryAttributable
    {
        private readonly AuthService _authService;

        public RegisterViewModel(AuthService authService)
        {
            _authService = authService;
        }

        [ObservableProperty] private string userRole;
        [ObservableProperty] private string name;
        [ObservableProperty] private string surname;
        [ObservableProperty] private string tcIdNumber;
        [ObservableProperty] private string phoneNumber;
        [ObservableProperty] private string password;

        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private string titleText = "Kayıt Ol";

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("role"))
            {
                UserRole = query["role"].ToString();
                TitleText = UserRole == "Teacher" ? "Öğretmen Kaydı" : "Veli Kaydı";
            }
        }

        [RelayCommand]
        private async Task RegisterAsync()
        {
            if (IsBusy) return;

            if (!ValidateInputs()) return;

            try
            {
                IsBusy = true;

                bool isSuccess = false;
                string message = "";
                string token = "";

                // 2. Rolüne göre doğru servisi çağır
                if (UserRole == "Teacher")
                {
                    var request = new TeacherRegisterRequest
                    {
                        Name = Name,
                        Surname = Surname,
                        TCIDNumber = TcIdNumber,
                        PhoneNumber = PhoneNumber,
                        Password = Password
                    };

                    var response = await _authService.RegisterTeacherAsync(request);
                    isSuccess = response.IsSuccess;
                    message = response.Message;
                    token = response.Token;
                }
                else
                {
                    var request = new ParentRegisterRequest
                    {
                        Name = Name,
                        Surname = Surname,
                        TCIDNumber = TcIdNumber,
                        PhoneNumber = PhoneNumber,
                        Password = Password
                    };

                    var response = await _authService.RegisterParentAsync(request);
                    isSuccess = response.IsSuccess;
                    message = response.Message;
                    token = response.Token;
                }

                IsBusy = false;

                // YÖNLENDİRME
                if (isSuccess)
                {
                    // Önce mesajı göster
                    await Shell.Current.DisplayAlert("Başarılı", message, "Tamam");

                    // Bilgileri kaydet
                    Preferences.Set("AuthToken", token);
                    Preferences.Set("UserRole", UserRole);
                    Preferences.Set("UserName", $"{Name} {Surname}");
                    
                    if (UserRole == "Teacher")
                    {
                        // Öğretmen ise sınıf oluşturma sayfasına git
                        await Shell.Current.GoToAsync("CreateClassPage");
                    }
                    else
                    {
                        // Veli ise ana sayfaya git
                        await Shell.Current.GoToAsync("//ParentHomePage");
                    }
 
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hata", message ?? "Kayıt yapılamadı.", "Tamam");
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Hata", $"Bağlantı hatası: {ex.Message}", "Tamam");
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Surname) ||
                string.IsNullOrWhiteSpace(TcIdNumber) || string.IsNullOrWhiteSpace(PhoneNumber) ||
                string.IsNullOrWhiteSpace(Password))
            {
                Shell.Current.DisplayAlert("Uyarı", "Lütfen tüm alanları doldurunuz.", "Tamam");
                return false;
            }

            if (TcIdNumber.Length != 11 || !long.TryParse(TcIdNumber, out _))
            {
                Shell.Current.DisplayAlert("Uyarı", "TC Kimlik No 11 haneli rakam olmalıdır.", "Tamam");
                return false;
            }

            if (PhoneNumber.Length != 11 || !PhoneNumber.StartsWith("0"))
            {
                Shell.Current.DisplayAlert("Uyarı", "Telefon numarası 11 haneli olmalı ve 0 ile başlamalıdır.", "Tamam");
                return false;
            }

            if (Password.Length < 8)
            {
                Shell.Current.DisplayAlert("Uyarı", "Şifre en az 8 karakter olmalıdır.", "Tamam");
                return false;
            }
            if (!Password.Any(char.IsUpper))
            {
                Shell.Current.DisplayAlert("Uyarı", "Şifre en az 1 büyük harf içermelidir.", "Tamam");
                return false;
            }
            if (!Password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                Shell.Current.DisplayAlert("Uyarı", "Şifre en az 1 özel karakter (nokta, ünlem vb.) içermelidir.", "Tamam");
                return false;
            }

            return true;
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using YuvaCep.Mobile.Dtos;
using YuvaCep.Mobile.Services;

namespace YuvaCep.Mobile.ViewModels
{
    public partial class ParentHomeViewModel : ObservableObject
    {
        private readonly UserService _userService;

        [ObservableProperty]
        private StudentDto student; // Öğrenci bilgisi burada tutulacak

        [ObservableProperty]
        private bool hasStudent; // Öğrenci var mı yok mu?

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string parentName;

        public ParentHomeViewModel(UserService userService)
        {
            _userService = userService;
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            ParentName = Preferences.Get("UserName", "Sayın Veli");

            await LoadStudentDataAsync();
        }

        [RelayCommand]
        public async Task LoadStudentDataAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var token = Preferences.Get("AuthToken", string.Empty);

                // Veriyi çekmeyi dene
                var result = await _userService.GetMyStudentAsync(token);

                if (result != null)
                {
                    Student = result;
                    HasStudent = true;
                }
                else
                {
                    Student = null;
                    HasStudent = false;
                }
            }
            catch (Exception ex)
            {
                // Hata olursa ne olduğunu görelim
                await Shell.Current.DisplayAlert("Hata", $"Veri çekilemedi: {ex.Message}", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }


        [RelayCommand]
        public async Task AddChildAsync()
        {
            // 1. Kullanıcıdan Kodu İste (Pop-up açılır)
            string code = await Shell.Current.DisplayPromptAsync(
                "Çocuk Ekle",
                "Lütfen kurumdan aldığınız 6 haneli öğrenci kodunu giriniz:",
                "Ekle",
                "İptal",
                "Örn: EB0903",
                maxLength: 6,
                keyboard: Keyboard.Text);

            // Eğer iptal ederse veya boş geçerse dur
            if (string.IsNullOrWhiteSpace(code)) return;

            if (IsBusy) return;
            IsBusy = true;

            // 2. Servise gönder
            var token = Preferences.Get("AuthToken", string.Empty);
            var result = await _userService.LinkStudentAsync(token, code);

            IsBusy = false;

            if (result.isSuccess)
            {
                await Shell.Current.DisplayAlert("Başarılı", result.message, "Tamam");
                // 3. Sayfayı Yenile (Bilgiler gelsin)
                await LoadStudentDataAsync();
            }
            else
            {
                await Shell.Current.DisplayAlert("Hata", result.message, "Tamam");
            }
        }

        [RelayCommand]
        public async Task LogoutAsync()
        {
            // 1. Kullanıcıya soralım 
            bool answer = await Shell.Current.DisplayAlert("Çıkış", "Hesabınızdan çıkış yapmak istiyor musunuz?", "Evet", "İptal");

            if (!answer) return;

            // 2. Hafızadaki bilgileri temizle 
            Preferences.Remove("AuthToken");
            Preferences.Remove("UserRole");
            Preferences.Remove("UserName"); 

            // 3. En başa (Rol Seçimi / Giriş) sayfasına gönder
            await Shell.Current.GoToAsync("//RoleSelectionPage");
        }

        [RelayCommand]
        private async Task GoToStudentDetailAsync()
        {
            if (Student == null) return;

            var navigationParameter = new Dictionary<string, object>
            {
                { "Student", Student }
            };

            // Yeni sayfaya git
            await Shell.Current.GoToAsync("StudentDetailPage", navigationParameter);
        }
    }
}
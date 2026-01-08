using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using YuvaCep.Mobile.Services;

namespace YuvaCep.Mobile.ViewModels
{
    public partial class ParentProfileViewModel : ObservableObject
    {
        private readonly ParentService _parentService;

        [ObservableProperty] private string userName;
        [ObservableProperty] private string phoneNumber;

        [ObservableProperty] private string currentPassword;
        [ObservableProperty] private string newPassword;
        [ObservableProperty] private string confirmPassword;
        [ObservableProperty] private Color themeColor;
        [ObservableProperty] private bool isBusy;

        public ParentProfileViewModel(ParentService parentService)
        {
            _parentService = parentService;
            Task.Run(LoadUserDataAsync);
            SetThemeColor();
        }

        private void SetThemeColor()
        {
            var role = Preferences.Get("UserRole", "");
            if (role == "Teacher")
            {
                ThemeColor = Color.FromArgb("#4A90E2");
            }
            else
            {
                ThemeColor = Color.FromArgb("#FF9F1C");
            }
        }


        public async Task LoadUserDataAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var localName = Preferences.Get("UserName", "Sayın Veli");
                if (!string.IsNullOrEmpty(localName)) UserName = localName;

                var profile = await _parentService.GetMyProfileAsync();

                if (profile != null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        UserName = $"{profile.Name} {profile.Surname}";
                        PhoneNumber = profile.PhoneNumber; 
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Veri yükleme hatası: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task UpdateContactInfoAsync()
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                await Shell.Current.DisplayAlert("Uyarı", "Telefon numarası boş olamaz.", "Tamam");
                return;
            }

            IsBusy = true;

            bool success = await _parentService.UpdateContactInfoAsync(PhoneNumber);

            IsBusy = false;

            if (success)
                await Shell.Current.DisplayAlert("Başarılı", "İletişim bilgileriniz güncellendi.", "Tamam");
            else
                await Shell.Current.DisplayAlert("Hata", "Güncelleme yapılamadı. Bağlantınızı kontrol edin.", "Tamam");
        }

        [RelayCommand]
        private async Task ChangePasswordAsync()
        {
            if (string.IsNullOrWhiteSpace(CurrentPassword) ||
                string.IsNullOrWhiteSpace(NewPassword) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await Shell.Current.DisplayAlert("Uyarı", "Lütfen tüm şifre alanlarını doldurun.", "Tamam");
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                await Shell.Current.DisplayAlert("Hata", "Yeni şifreler birbiriyle uyuşmuyor.", "Tamam");
                return;
            }

            IsBusy = true;
            var result = await _parentService.ChangePasswordAsync(CurrentPassword, NewPassword);
            IsBusy = false;

            if (result.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Başarılı", result.Message, "Tamam");
                CurrentPassword = ""; NewPassword = ""; ConfirmPassword = "";
            }
            else
            {
                await Shell.Current.DisplayAlert("Hata", result.Message, "Tamam");
            }
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            try
            {
                if (Shell.Current.Navigation.NavigationStack.Count > 1)
                {
                    await Shell.Current.Navigation.PopAsync();
                }
                else
                {
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch
            {
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
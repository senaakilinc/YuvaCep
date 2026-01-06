using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using YuvaCep.Mobile.Dtos;
using YuvaCep.Mobile.Services;

namespace YuvaCep.Mobile.ViewModels
{
    public partial class ParentHomeViewModel : ObservableObject
    {
        private readonly StudentService _studentService;

        [ObservableProperty] private string parentName;
        [ObservableProperty] private string referenceCodeInput; 
        [ObservableProperty] private bool isBusy;

        public ObservableCollection<ChildDto> MyChildren { get; } = new();

        public ParentHomeViewModel(StudentService studentService)
        {
            _studentService = studentService;
            LoadData();
        }

        public async void LoadData()
        {
            ParentName = Preferences.Get("UserName", "Değerli Velimiz");
            await RefreshChildrenAsync();
        }

        [RelayCommand]
        private async Task RefreshChildrenAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var list = await _studentService.GetMyChildrenAsync();
                MyChildren.Clear();
                if (list != null)
                {
                    foreach (var child in list) MyChildren.Add(child);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Hata: {ex.Message}");
            }

            finally { IsBusy = false; }
        }

        [RelayCommand]
        private async Task LinkStudentAsync()
        {
            if (string.IsNullOrWhiteSpace(ReferenceCodeInput))
            {
                await Shell.Current.DisplayAlert("Uyarı", "Lütfen bir kod giriniz.", "Tamam");
                return;
            }

            IsBusy = true;
            string result = await _studentService.LinkStudentAsync(ReferenceCodeInput.Trim().ToUpper());
            IsBusy = false;

            if (result == "OK")
            {
                await Shell.Current.DisplayAlert("Harika!", "Çocuğunuz başarıyla eklendi.", "Tamam");
                ReferenceCodeInput = "";
                await RefreshChildrenAsync(); 
            }
            else
            {
                await Shell.Current.DisplayAlert("Hata", result, "Tamam");
            }
        }

        [RelayCommand]
        private async Task GoToChildCards(ChildDto child)
        {
            if (child == null) return;
            await Shell.Current.GoToAsync($"StudentCards_Route?studentId={child.Id}&name={child.Name}");
        }

        [RelayCommand]
        private async Task LogoutAsync()
        {
            bool answer = await Shell.Current.DisplayAlert("Çıkış", "Çıkış yapmak istiyor musunuz?", "Evet", "Hayır");
            if (answer)
            {
                SecureStorage.Remove("auth_token");
                Preferences.Clear();
                await Shell.Current.GoToAsync("RoleSelection_Route");
            }
        }
    }
}
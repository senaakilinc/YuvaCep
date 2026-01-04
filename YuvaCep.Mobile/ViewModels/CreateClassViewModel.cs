using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using YuvaCep.Mobile.Services;

namespace YuvaCep.Mobile.ViewModels
{
    public partial class CreateClassViewModel : ObservableObject
    {
        private readonly ClassService _classService;

        public CreateClassViewModel(ClassService classService)
        {
            _classService = classService;
        }

        [ObservableProperty]
        private string className;

        [ObservableProperty]
        private string selectedAgeGroup;

        public List<string> AgeGroups { get; } = new List<string>
        {
            "3-4 Yaş Grubu",
            "4-5 Yaş Grubu",
            "5-6 Yaş Grubu"
        };

        [ObservableProperty]
        private bool isBusy;

        [RelayCommand]
        private async Task CreateClassAsync()
        {
            if (IsBusy) return;

            if (string.IsNullOrWhiteSpace(ClassName) || string.IsNullOrWhiteSpace(SelectedAgeGroup))
            {
                await Shell.Current.DisplayAlert("Uyarı", "Lütfen sınıf adı ve yaş grubu seçiniz.", "Tamam");
                return;
            }

            try
            {
                IsBusy = true;

                var isSuccess = await _classService.CreateClassAsync(ClassName, SelectedAgeGroup);

                if (isSuccess)
                {
                    Preferences.Set("ClassName", ClassName);

                    await Shell.Current.DisplayAlert("Başarılı", "Sınıfınız oluşturuldu!", "Tamam");
                    await Shell.Current.GoToAsync("//TeacherHomePage");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hata", "Sınıf oluşturulurken bir hata oluştu.", "Tamam");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
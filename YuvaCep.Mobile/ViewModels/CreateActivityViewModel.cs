using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using YuvaCep.Mobile.Enums;
using YuvaCep.Mobile.Services;

namespace YuvaCep.Mobile.ViewModels
{
    public partial class CreateActivityViewModel : ObservableObject
    {
        private readonly ActivityService _activityService;

        [ObservableProperty] private string title;
        [ObservableProperty] private ThemeItem selectedThemeItem;
        [ObservableProperty] private DateTime selectedDate = DateTime.Now;
        [ObservableProperty] private bool isBusy;

        public List<ThemeItem> Themes { get; } = new List<ThemeItem>
        {
            new ThemeItem { Name = "Kişisel Bakım (Diş/El Yıkama) 🦷", Value = ChartTheme.Hygiene },
            new ThemeItem { Name = "Spor & Egzersiz ⚽", Value = ChartTheme.Sport },
            new ThemeItem { Name = "Eğitim & Kitap Okuma 📚", Value = ChartTheme.Education },
            new ThemeItem { Name = "Sanat & Yaratıcılık 🎨", Value = ChartTheme.Art },
            new ThemeItem { Name = "Sağlıklı Beslenme 🥕", Value = ChartTheme.Nutrition },
            new ThemeItem { Name = "Genel / Diğer 📅", Value = ChartTheme.Generic }
        };

        public CreateActivityViewModel(ActivityService activityService)
        {
            _activityService = activityService;
            SelectedThemeItem = Themes.FirstOrDefault();
        }

        [RelayCommand]
        private async Task CreateActivityAsync()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                await Shell.Current.DisplayAlert("Uyarı", "Lütfen bir başlık girin.", "Tamam");
                return;
            }

            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var token = Preferences.Get("AuthToken", string.Empty);
                var themeValue = (int)SelectedThemeItem.Value;

                var success = await _activityService.CreateActivityChartAsync(
                    token,
                    Title,
                    themeValue,
                    SelectedDate.Month,
                    SelectedDate.Year
                );

                if (success)
                {
                    await Shell.Current.DisplayAlert("Başarılı", "Çizelge oluşturuldu!", "Tamam");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hata", "Oluşturulamadı. İnternet bağlantınızı kontrol edin.", "Tamam");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", ex.Message, "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public class ThemeItem
        {
        public string Name { get; set; }
        public ChartTheme Value { get; set; }
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
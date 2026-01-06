using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using YuvaCep.Mobile.Dtos;
using YuvaCep.Mobile.Services;

namespace YuvaCep.Mobile.ViewModels
{
    public partial class ActivityChartsListViewModel : ObservableObject
    {
        private readonly ActivityService _activityService;

        [ObservableProperty] private bool isBusy;

        public ObservableCollection<ActivityChartDto> Charts { get; } = new();

        public ActivityChartsListViewModel(ActivityService activityService)
        {
            _activityService = activityService;
        }

        [RelayCommand]
        public async Task LoadChartsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var token = Preferences.Get("AuthToken", string.Empty);
                var data = await _activityService.GetTeacherChartsAsync(token, 1, 2026);

                Charts.Clear();
                if (data != null)
                {
                    foreach (var item in data) Charts.Add(item);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Liste yüklenirken hata: {ex.Message}", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task AddNewAsync()
        {
            await Shell.Current.GoToAsync("CreateActivityPage");
        }

        [RelayCommand]
        private async Task GoToDetailAsync(ActivityChartDto chart)
        {
            if (chart == null) return;
            await Shell.Current.GoToAsync($"ActivityChartDetailPage?ChartId={chart.Id}");
        }

        [RelayCommand]
        private async Task DeleteChartAsync(ActivityChartDto chart)
        {
            if (chart == null) return;

            bool answer = await Shell.Current.DisplayAlert("Sil", $"{chart.Title} silinsin mi?", "Evet", "Hayır");
            if (!answer) return;

            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var token = Preferences.Get("AuthToken", string.Empty);
                bool success = await _activityService.DeleteChartAsync(token, chart.Id);

                if (success)
                {
                    var itemToRemove = Charts.FirstOrDefault(x => x.Id == chart.Id);
                    if (itemToRemove != null)
                    {
                        Charts.Remove(itemToRemove);
                    }
                    await Shell.Current.DisplayAlert("Başarılı", "Çizelge silindi.", "Tamam");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hata", "Silme işlemi başarısız oldu.", "Tamam");
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
    }
}
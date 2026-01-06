using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using YuvaCep.Mobile.Dtos;
using YuvaCep.Mobile.Services;

namespace YuvaCep.Mobile.ViewModels
{
    [QueryProperty(nameof(StudentId), "studentId")]
    public partial class StudentChartsListViewModel : ObservableObject
    {
        private readonly ActivityService _activityService;

        [ObservableProperty] private string studentId;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private string statusMessage;

        public ObservableCollection<ActivityChartDto> Charts { get; } = new();

        public StudentChartsListViewModel(ActivityService activityService)
        {
            _activityService = activityService;
            StatusMessage = "Veriler bekleniyor...";
        }

        partial void OnStudentIdChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                MainThread.BeginInvokeOnMainThread(async () => await LoadChartsAsync());
            }
        }

        [RelayCommand]
        public async Task LoadChartsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            StatusMessage = "Yükleniyor...";

            try
            {
                if (Guid.TryParse(StudentId, out Guid sId))
                {
                    var token = Preferences.Get("AuthToken", string.Empty);
                    var currentMonth = DateTime.Now.Month;
                    var currentYear = DateTime.Now.Year;

                    var data = await _activityService.GetStudentChartsAsync(token, sId.ToString(), currentMonth, currentYear);

                    Charts.Clear();
                    if (data != null && data.Count > 0)
                    {
                        foreach (var item in data) Charts.Add(item);
                        StatusMessage = "";
                    }
                    else
                    {
                        StatusMessage = "⚠️ Bu ay için atanmış bir aktivite yok.";
                    }
                }
                else
                {
                    StatusMessage = "Hata: Geçersiz Öğrenci ID.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"HATA: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToDetailAsync(ActivityChartDto chart)
        {
            if (chart == null) return;

            await Shell.Current.GoToAsync($"StudentChartDetailPage?ChartId={chart.Id}&Title={chart.Title}&StudentId={StudentId}");
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
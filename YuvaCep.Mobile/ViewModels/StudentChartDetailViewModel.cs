using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using YuvaCep.Mobile.Dtos;
using YuvaCep.Mobile.Services;
using YuvaCep.Mobile.Enums;


namespace YuvaCep.Mobile.ViewModels
{
    [QueryProperty(nameof(ChartId), "ChartId")]
    [QueryProperty(nameof(StudentId), "StudentId")]
    public partial class StudentChartDetailViewModel : ObservableObject
    {
        private readonly ActivityService _activityService;

        [ObservableProperty] private string studentId;
        [ObservableProperty] private string chartId;

        [ObservableProperty] private ActivityChartDetailDto chartDetail;
        [ObservableProperty] private ObservableCollection<StudentDayModel> days;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private string pageTitle;

        [ObservableProperty] private string completeButtonText = "✅ BUGÜNÜ TAMAMLADIM";
        [ObservableProperty] private Color completeButtonColor = Color.FromArgb("#4CAF50"); // Yeşil
        [ObservableProperty] private bool isCompleteButtonEnabled = true;

        public StudentChartDetailViewModel()
        {
            _activityService = new ActivityService();
            Days = new ObservableCollection<StudentDayModel>();
        }

        partial void OnChartIdChanged(string value) => CheckAndLoad();
        partial void OnStudentIdChanged(string value) => CheckAndLoad();

        private void CheckAndLoad()
        {
            if (!string.IsNullOrEmpty(ChartId) && !string.IsNullOrEmpty(StudentId))
                LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var id = Guid.Parse(ChartId);
                var sId = Guid.Parse(StudentId);
                var token = Preferences.Get("AuthToken", "");

                var data = await _activityService.GetStudentChartDetailsAsync(token, id, sId);

                if (data != null)
                {
                    ChartDetail = data;
                    PageTitle = data.Title;
                    GenerateCalendar(data);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", ex.Message, "Tamam");
            }
            finally { IsBusy = false; }
        }

        private void GenerateCalendar(ActivityChartDetailDto data)
        {
            Days.Clear();
            int daysInMonth = DateTime.DaysInMonth(data.Year, data.Month);
            DateTime today = DateTime.Today;

            bool isTodayDone = data.CompletedDates.Any(d => d.Date == today);

            UpdateButtonState(isTodayDone);

            for (int i = 1; i <= daysInMonth; i++)
            {
                DateTime currentDayDate = new DateTime(data.Year, data.Month, i);

                bool isDone = data.CompletedDates.Any(d => d.Date == currentDayDate.Date);

                Color boxColor;
                Color textColor;

                if (isDone)
                {
                    boxColor = Colors.Green;
                    textColor = Colors.White;
                }
                else if (currentDayDate < today)
                {
                    boxColor = Colors.Red;
                    textColor = Colors.White;
                }
                else
                {
                    boxColor = Color.FromArgb("#E0E0E0");
                    textColor = Colors.Black;
                }

                Days.Add(new StudentDayModel
                {
                    DayNumber = i,
                    Date = currentDayDate,
                    IsCompleted = isDone,
                    BoxColor = boxColor,
                    TextColor = textColor
                });
            }
        }

        private void UpdateButtonState(bool isDone)
        {
            if (isDone)
            {
                CompleteButtonText = "YARIN GÖRÜŞÜRÜZ 👋"; 
                CompleteButtonColor = Colors.Gray;
                IsCompleteButtonEnabled = false; 
            }
            else
            {
                CompleteButtonText = "✅ BUGÜNÜ TAMAMLADIM";
                CompleteButtonColor = Color.FromArgb("#4CAF50");
                IsCompleteButtonEnabled = true; 
            }
        }

        [RelayCommand]
        private async Task CompleteTodayAsync()
        {
            bool answer = await Shell.Current.DisplayAlert("Tamamla", "Bugünkü görevi tamamladın mı?", "Evet", "Hayır");
            if (!answer) return;

            if (ChartDetail == null) return;
            IsBusy = true;
            try
            {
                var token = Preferences.Get("AuthToken", "");
                var success = await _activityService.CompleteChartAsync(token, ChartDetail.Id, ChartDetail.StudentId, DateTime.Now);
                if (success)
                {
                    var todayItem = Days.FirstOrDefault(d => d.Date.Date == DateTime.Today);
                    if (todayItem != null)
                    {
                        todayItem.IsCompleted = true;
                        todayItem.BoxColor = Colors.Green;
                        todayItem.TextColor = Colors.White;
                    }

                    UpdateButtonState(true);

                    await Shell.Current.DisplayAlert("Harika!", "Bugünü başarıyla tamamladın.", "Tamam");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hata", "Kaydedilemedi.", "Tamam");
                }
            }
            finally { IsBusy = false; }
        }

        [RelayCommand]
        private async Task GoBackAsync() => await Shell.Current.GoToAsync("..");
    }

    public partial class StudentDayModel : ObservableObject
    {
        public int DayNumber { get; set; }
        public DateTime Date { get; set; }

        [ObservableProperty] private bool isCompleted;
        [ObservableProperty] private Color boxColor;
        [ObservableProperty] private Color textColor;
    }
}
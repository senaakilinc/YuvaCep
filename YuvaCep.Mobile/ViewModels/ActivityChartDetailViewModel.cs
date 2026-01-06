using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views; 
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using YuvaCep.Mobile.Dtos;
using YuvaCep.Mobile.Services;
using YuvaCep.Mobile.Views.Popups; 

namespace YuvaCep.Mobile.ViewModels
{
    [QueryProperty(nameof(ChartId), "ChartId")]
    public partial class ActivityChartDetailViewModel : ObservableObject
    {
        private readonly ActivityService _activityService;

        [ObservableProperty] private string chartId;
        [ObservableProperty] private string chartTitle;
        [ObservableProperty] private ObservableCollection<ChartStudentDetailDto> students;
        [ObservableProperty] private bool isBusy;

        private int _month;
        private int _year;

        public ActivityChartDetailViewModel(ActivityService activityService)
        {
            _activityService = activityService;
            Students = new ObservableCollection<ChartStudentDetailDto>();
        }

        partial void OnChartIdChanged(string value)
        {
            if (!string.IsNullOrEmpty(value)) LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var token = Preferences.Get("AuthToken", string.Empty);
                var detail = await _activityService.GetTeacherChartDetailsAsync(token, Guid.Parse(ChartId));

                if (detail != null)
                {
                    ChartTitle = detail.Title;
                    _month = detail.Month;
                    _year = detail.Year;

                    Students.Clear();
                    if (detail.Students != null)
                    {
                        foreach (var st in detail.Students) Students.Add(st);
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", ex.Message, "Tamam");
            }
            finally { IsBusy = false; }
        }

        [RelayCommand]
        private void OpenStudentPopup(ChartStudentDetailDto student)
        {
            if (student == null) return;


            var dayList = GenerateCalendarList(student.CompletedDates, _month, _year);

            var popup = new CalendarPopup(dayList, student.StudentName);

            Shell.Current.CurrentPage.ShowPopup(popup);
        }

        private List<StudentDayModel> GenerateCalendarList(List<DateTime> completedDates, int month, int year)
        {
            var list = new List<StudentDayModel>();

            if (month == 0 || year == 0)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }

            int daysInMonth = DateTime.DaysInMonth(year, month);
            DateTime today = DateTime.Today;

            for (int i = 1; i <= daysInMonth; i++)
            {
                DateTime currentDayDate = new DateTime(year, month, i);

                bool isDone = completedDates != null && completedDates.Any(d => d.Date == currentDayDate.Date);

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

                list.Add(new StudentDayModel
                {
                    DayNumber = i,
                    Date = currentDayDate,
                    IsCompleted = isDone,
                    BoxColor = boxColor,
                    TextColor = textColor
                });
            }
            return list;
        }

        [RelayCommand]
        private async Task GoBackAsync() => await Shell.Current.GoToAsync("..");
    }
}
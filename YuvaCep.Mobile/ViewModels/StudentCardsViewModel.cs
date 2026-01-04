using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using YuvaCep.Mobile.Dtos;
using YuvaCep.Mobile.Services;

namespace YuvaCep.Mobile.ViewModels
{
    [QueryProperty(nameof(StudentId), "studentId")]
    public partial class StudentCardsViewModel : ObservableObject
    {
        private readonly StudentService _studentService;

        [ObservableProperty] private string studentId;
        [ObservableProperty] string studentName;
        [ObservableProperty] string className;
        [ObservableProperty] bool isBusy;

        public bool IsTeacher => Preferences.Get("UserRole", "") == "Teacher";
        public bool IsParent => !IsTeacher;

        public StudentCardsViewModel()
        {
            _studentService = new StudentService();
        }

        partial void OnStudentIdChanged(string value)
        {
            if (!string.IsNullOrEmpty(value) && Guid.TryParse(value, out Guid id))
            {
                LoadStudentInfo(id);
            }
        }

        private async void LoadStudentInfo(Guid id)
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var data = await _studentService.GetStudentDetailAsync(id);
                if (data != null)
                {
                    StudentName = string.IsNullOrEmpty(data.Name) ? $"{data.FirstName} {data.LastName}" : data.Name;
                    ClassName = data.ClassName; 
                }
            }
            finally { IsBusy = false; }
        }

        [RelayCommand]
        private async Task GoToTeacherDailyReportAsync()
        {
            await Shell.Current.GoToAsync($"TeacherDailyReport_Route?studentId={StudentId}");
        }

        [RelayCommand]
        private async Task GoToDailyReportAsync()
        {
            await Shell.Current.GoToAsync($"DailyReport_Route?studentId={StudentId}");
        }

        [RelayCommand]
        private async Task GoToFoodListAsync()
        {
            await Shell.Current.GoToAsync($"FoodList_Route?studentId={StudentId}");
        }

        [RelayCommand]
        private async Task GoToLessonProgramAsync()
        {
            if (!string.IsNullOrEmpty(StudentId))
            {
                await Shell.Current.GoToAsync($"CurriculumPage?studentId={StudentId}");
            }
        }

        [RelayCommand]
        private async Task GoToAnnouncementsAsync()
        {
            await Shell.Current.GoToAsync($"Announcements_Route?studentId={StudentId}");
        }

        [RelayCommand]
        private async Task GoToBadgeDetailsAsync()
        {
            await Shell.Current.GoToAsync("BadgeDetail_Route");
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using YuvaCep.Mobile.Dtos;
using YuvaCep.Mobile.Enums;
using YuvaCep.Mobile.Services;

namespace YuvaCep.Mobile.ViewModels
{
    public partial class TeacherDailyReportViewModel : ObservableObject
    {
        private readonly DailyReportService _reportService;
        private readonly StudentService _studentService;

        // --- ÖĞRENCİ SEÇİMİ ---
        public ObservableCollection<StudentListDto> Students { get; } = new();

        [ObservableProperty]
        private StudentListDto selectedStudent;

        [ObservableProperty]
        private bool isBusy;

        // --- FORM VERİLERİ ---
        public List<FoodStatus> FoodList => Enum.GetValues(typeof(FoodStatus)).Cast<FoodStatus>().ToList();
        public List<SleepStatus> SleepList => Enum.GetValues(typeof(SleepStatus)).Cast<SleepStatus>().ToList();
        public List<ActivityStatus> ActivityList => Enum.GetValues(typeof(ActivityStatus)).Cast<ActivityStatus>().ToList();

        [ObservableProperty] private MoodStatus selectedMood = MoodStatus.Mutlu;
        [ObservableProperty] private string moodNote;
        [ObservableProperty] private FoodStatus selectedBreakfast = FoodStatus.HepsiniYedi;
        [ObservableProperty] private FoodStatus selectedLunch = FoodStatus.HepsiniYedi;
        [ObservableProperty] private string foodNote;
        [ObservableProperty] private SleepStatus selectedSleep = SleepStatus.Uyudu;
        [ObservableProperty] private ActivityStatus selectedActivity = ActivityStatus.Katildi;
        [ObservableProperty] private string activityNote;
        [ObservableProperty] private string teacherNote;

        public TeacherDailyReportViewModel(DailyReportService reportService, StudentService studentService)
        {
            _reportService = reportService;
            _studentService = studentService;
            LoadStudentsAsync();
        }

        private async void LoadStudentsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var token = Preferences.Get("AuthToken", string.Empty);
                var allStudents = await _studentService.GetMyStudentsAsync();
                var reportedStudentIds = await _reportService.GetReportedStudentIdsAsync(token);

                Students.Clear();
                foreach (var student in allStudents)
                {

                    if (!reportedStudentIds.Contains(student.Id))
                    {
                        student.IsSelected = false; // Başlangıçta seçili değil
                        Students.Add(student);
                    }
                }

                if (Students.Count == 0)
                {
                    await Shell.Current.DisplayAlert("Bilgi", "Bugün için tüm sınıfın raporu tamamlanmış! 🎉", "Tamam");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Liste yüklenemedi: {ex.Message}", "Tamam");
            }
            finally { IsBusy = false; }
        }

        [RelayCommand]
        private void SelectStudent(StudentListDto student)
        {
            if (student == null) return;

            foreach (var s in Students)
            {
                s.IsSelected = false;
            }

            student.IsSelected = true;
            SelectedStudent = student; 
        }

        [RelayCommand]
        private void SetMood(MoodStatus mood) => SelectedMood = mood;

        [RelayCommand]
        public async Task SaveReportAsync()
        {
            if (SelectedStudent == null) return;
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var newReport = new DailyReportDto
                {
                    Id = SelectedStudent.Id,
                    Date = DateTime.Now,
                    Mood = SelectedMood,
                    MoodNote = MoodNote ?? "",
                    Breakfast = SelectedBreakfast,
                    Lunch = SelectedLunch,
                    FoodNote = FoodNote ?? "",
                    Sleep = SelectedSleep,
                    Activity = SelectedActivity,
                    ActivityNote = ActivityNote ?? "",
                    TeacherNote = TeacherNote ?? ""
                };

                var token = Preferences.Get("AuthToken", string.Empty);
                var result = await _reportService.AddReportAsync(token, newReport);

                if (result)
                {
                    await Shell.Current.DisplayAlert("Başarılı", $"{SelectedStudent.Name} raporlandı.", "Tamam");

                    var studentToRemove = SelectedStudent;
                    SelectedStudent = null; 
                    Students.Remove(studentToRemove); 

                    ResetForm();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hata", "Rapor kaydedilemedi.", "Tamam");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Hata: {ex.Message}", "Tamam");
            }
            finally { IsBusy = false; }
        }

        private void ResetForm()
        {
            MoodNote = "";
            FoodNote = "";
            ActivityNote = "";
            TeacherNote = "";
            SelectedMood = MoodStatus.Mutlu;
            SelectedBreakfast = FoodStatus.HepsiniYedi;
            SelectedLunch = FoodStatus.HepsiniYedi;
            SelectedSleep = SleepStatus.Uyudu;
            SelectedActivity = ActivityStatus.Katildi;
        }

        [RelayCommand]
        private async Task GoBackAsync() => await Shell.Current.GoToAsync("..");
    }
}
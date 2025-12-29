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
        public ObservableCollection<StudentDto> Students { get; } = new();

        [ObservableProperty]
        private StudentDto selectedStudent; // Listeden seçilen öğrenci

        [ObservableProperty]
        private bool isBusy;

        // --- FORM VERİLERİ ---
        public List<FoodStatus> FoodList => Enum.GetValues(typeof(FoodStatus)).Cast<FoodStatus>().ToList();
        public List<SleepStatus> SleepList => Enum.GetValues(typeof(SleepStatus)).Cast<SleepStatus>().ToList();
        public List<ActivityStatus> ActivityList => Enum.GetValues(typeof(ActivityStatus)).Cast<ActivityStatus>().ToList();

        // Seçilen Değerler
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

            // Sayfa açılınca öğrencileri yükle
            LoadStudentsAsync();
        }

        private async void LoadStudentsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var token = Preferences.Get("AuthToken", string.Empty);

                // Tüm sınıf listesini çek
                var allStudents = await _studentService.GetAllStudentsAsync(token);

                // Bugün raporu girilmiş olanların ID listesini çek
                var reportedStudentIds = await _reportService.GetReportedStudentIdsAsync(token);

                Students.Clear();

                foreach (var student in allStudents)
                {
                    // FİLTRELEME
                    if (!reportedStudentIds.Contains(student.Id))
                    {
                        Students.Add(student);
                    }
                }

                // Eğer herkesin raporu girildiyse ve liste boş kaldıysa:
                if (Students.Count == 0)
                {
                    await Shell.Current.DisplayAlert("Bilgi", "Bugün için tüm öğrencilerin raporu girilmiş! 🎉", "Tamam");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Liste yüklenemedi: {ex.Message}", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }


        [RelayCommand]
        public async Task SaveReportAsync()
        {
            if (SelectedStudent == null)
            {
                await Shell.Current.DisplayAlert("Uyarı", "Lütfen önce bir öğrenci seçin!", "Tamam");
                return;
            }

            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var newReport = new DailyReportDto
                {
                    Id = SelectedStudent.Id, // Seçilen öğrencinin ID'si
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
                    await Shell.Current.DisplayAlert("Başarılı", $"{SelectedStudent.Name} için rapor kaydedildi.", "Tamam");
                    Students.Remove(SelectedStudent);
                    SelectedStudent = null;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hata", "Rapor kaydedilemedi.", "Tamam");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Bir sorun oluştu: {ex.Message}", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private void SetMood(MoodStatus mood)
        {
            // Tıklanan modu seçili mod olarak ayarla
            SelectedMood = mood;
        }

        [RelayCommand]
        private async Task GoBackAsync() => await Shell.Current.GoToAsync("..");
    }
}
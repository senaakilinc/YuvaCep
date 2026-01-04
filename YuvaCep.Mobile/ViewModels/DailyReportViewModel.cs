using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using YuvaCep.Mobile.Dtos;
using YuvaCep.Mobile.Enums;
using YuvaCep.Mobile.Services;

namespace YuvaCep.Mobile.ViewModels
{
    [QueryProperty(nameof(StudentId), "studentId")]
    public partial class DailyReportViewModel : ObservableObject
    {
        private readonly DailyReportService _reportService;
        private readonly StudentService _studentService;

        private List<DailyReportDto> _allReports = new();

        [ObservableProperty] private string studentId;
        [ObservableProperty] private string studentName;
        [ObservableProperty] private string className;

        [ObservableProperty] private DailyReportDto currentReport;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private bool hasReport;
        [ObservableProperty] private bool isReportEmpty;

        [ObservableProperty]
        private DateTime selectedDate = DateTime.Now;

        public string MoodEmoji => CurrentReport?.Mood switch { MoodStatus.Harika => "🤩", MoodStatus.Mutlu => "🙂", MoodStatus.Normal => "😐", MoodStatus.Uzgun => "☹️", MoodStatus.CokUzgun => "😭", _ => "❓" };
        public string MoodText => CurrentReport?.Mood.ToString() ?? "-";
        public Color MoodColor => CurrentReport?.Mood switch { MoodStatus.Harika or MoodStatus.Mutlu => Colors.Green, MoodStatus.Normal => Colors.Orange, _ => Colors.Red };

        public string BreakfastText => CurrentReport?.Breakfast switch { FoodStatus.HepsiniYedi => "Hepsini Yedi", FoodStatus.YarisiniYedi => "Yarısını Yedi", FoodStatus.AzYedi => "Az Yedi", FoodStatus.Yemedi => "Yemedi", _ => "-" };
        public string LunchText => CurrentReport?.Lunch switch { FoodStatus.HepsiniYedi => "Hepsini Yedi", FoodStatus.YarisiniYedi => "Yarısını Yedi", FoodStatus.AzYedi => "Az Yedi", FoodStatus.Yemedi => "Yemedi", _ => "-" };

        public string SleepText => CurrentReport?.Sleep == SleepStatus.Uyudu ? "Uyudu 😴" : "Uyumadı 😳";
        public string ActivityText => CurrentReport?.Activity switch { ActivityStatus.Katildi => "Katıldı ✅", ActivityStatus.KismenKatildi => "Kısmen Katıldı ⚠️", ActivityStatus.Katilmadi => "Katılmadı ❌", _ => "-" };

        public string MoodNoteDisplay => string.IsNullOrWhiteSpace(CurrentReport?.MoodNote) ? "Günün modu hakkında yorum eklenmedi." : CurrentReport.MoodNote;
        public string FoodNoteDisplay => string.IsNullOrWhiteSpace(CurrentReport?.FoodNote) ? "Yemek hakkında yorum eklenmedi." : CurrentReport.FoodNote;
        public string ActivityNoteDisplay => string.IsNullOrWhiteSpace(CurrentReport?.ActivityNote) ? "Etkinlik hakkında yorum eklenmedi." : CurrentReport.ActivityNote;
        public string TeacherNoteDisplay => string.IsNullOrWhiteSpace(CurrentReport?.TeacherNote) ? "Genel bir not eklenmedi." : CurrentReport.TeacherNote;
        public Color NoteColor(string note) => string.IsNullOrWhiteSpace(note) ? Colors.Gray : Colors.Black;
        public Color MoodNoteColor => NoteColor(CurrentReport?.MoodNote);
        public Color FoodNoteColor => NoteColor(CurrentReport?.FoodNote);
        public Color ActivityNoteColor => NoteColor(CurrentReport?.ActivityNote);
        public Color TeacherNoteColor => NoteColor(CurrentReport?.TeacherNote);


        public DailyReportViewModel(DailyReportService reportService)
        {
            _reportService = reportService;
            _studentService = new StudentService(); 
        }

        partial void OnStudentIdChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await LoadStudentInfoAsync(); 
                    await LoadAllReportsAsync();  
                });
            }
        }

        partial void OnSelectedDateChanged(DateTime value)
        {
            FilterReportByDate();
        }

        private async Task LoadStudentInfoAsync()
        {
            if (Guid.TryParse(StudentId, out Guid guidId))
            {
                var studentDetail = await _studentService.GetStudentDetailAsync(guidId);
                if (studentDetail != null)
                {
                    StudentName = studentDetail.FullName;
                    ClassName = studentDetail.ClassName;
                }
            }
        }

        [RelayCommand]
        public async Task LoadAllReportsAsync()
        {
            if (IsBusy || string.IsNullOrEmpty(StudentId)) return;
            IsBusy = true;

            try
            {
                var token = Preferences.Get("AuthToken", string.Empty);

                if (Guid.TryParse(StudentId, out Guid guidId))
                {
                    _allReports = await _reportService.GetStudentReportsAsync(token, guidId);
                    FilterReportByDate();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Rapor Yükleme Hatası: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void FilterReportByDate()
        {
            if (_allReports == null || !_allReports.Any())
            {
                SetReport(null);
                return;
            }

            var reportForDate = _allReports.FirstOrDefault(r => r.Date.Date == SelectedDate.Date);
            SetReport(reportForDate);
        }

        private void SetReport(DailyReportDto report)
        {
            CurrentReport = report;
            HasReport = report != null;
            IsReportEmpty = !HasReport;

            OnPropertyChanged(nameof(MoodEmoji));
            OnPropertyChanged(nameof(MoodText));
            OnPropertyChanged(nameof(MoodColor));
            OnPropertyChanged(nameof(BreakfastText));
            OnPropertyChanged(nameof(LunchText));
            OnPropertyChanged(nameof(SleepText));
            OnPropertyChanged(nameof(ActivityText));
            OnPropertyChanged(nameof(MoodNoteDisplay));
            OnPropertyChanged(nameof(FoodNoteDisplay));
            OnPropertyChanged(nameof(ActivityNoteDisplay));
            OnPropertyChanged(nameof(TeacherNoteDisplay));
            OnPropertyChanged(nameof(MoodNoteColor));
            OnPropertyChanged(nameof(FoodNoteColor));
            OnPropertyChanged(nameof(ActivityNoteColor));
            OnPropertyChanged(nameof(TeacherNoteColor));
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
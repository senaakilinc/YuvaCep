using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using YuvaCep.Mobile.Dtos;
using YuvaCep.Mobile.Enums;
using YuvaCep.Mobile.Services;

namespace YuvaCep.Mobile.ViewModels
{
    [QueryProperty(nameof(Student), "Student")]
    public partial class DailyReportViewModel : ObservableObject
    {
        private readonly DailyReportService _reportService;

        // Tüm raporlar burada (Önbellek)
        private List<DailyReportDto> _allReports = new();

        [ObservableProperty]
        private StudentDto student;

        [ObservableProperty]
        private DailyReportDto currentReport; // Ekranda gösterilen rapor

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool hasReport; // O gün rapor var mı?

        [ObservableProperty]
        private bool isReportEmpty; // O gün rapor YOK mu? (Tersi)

        // TARİH SEÇİMİ
        [ObservableProperty]
        private DateTime selectedDate = DateTime.Now;

        // --- GÖRSEL ÖZELLİKLER (CurrentReport değiştikçe güncellenir) ---
        public string MoodEmoji => CurrentReport?.Mood switch { MoodStatus.Harika => "🤩", MoodStatus.Mutlu => "🙂", MoodStatus.Normal => "😐", MoodStatus.Uzgun => "☹️", MoodStatus.CokUzgun => "😭", _ => "❓" };
        public string MoodText => CurrentReport?.Mood.ToString() ?? "-";
        public Color MoodColor => CurrentReport?.Mood switch { MoodStatus.Harika or MoodStatus.Mutlu => Colors.Green, MoodStatus.Normal => Colors.Orange, _ => Colors.Red };

        public string BreakfastText => CurrentReport?.Breakfast switch { FoodStatus.HepsiniYedi => "Hepsini Yedi", FoodStatus.YarisiniYedi => "Yarısını Yedi", FoodStatus.AzYedi => "Az Yedi", FoodStatus.Yemedi => "Yemedi", _ => "-" };
        public string LunchText => CurrentReport?.Lunch switch { FoodStatus.HepsiniYedi => "Hepsini Yedi", FoodStatus.YarisiniYedi => "Yarısını Yedi", FoodStatus.AzYedi => "Az Yedi", FoodStatus.Yemedi => "Yemedi", _ => "-" };

        public string SleepText => CurrentReport?.Sleep == SleepStatus.Uyudu ? "Uyudu 😴" : "Uyumadı 😳";
        public string ActivityText => CurrentReport?.Activity switch { ActivityStatus.Katildi => "Katıldı ✅", ActivityStatus.KismenKatildi => "Kısmen Katıldı ⚠️", ActivityStatus.Katilmadi => "Katılmadı ❌", _ => "-" };


        public DailyReportViewModel(DailyReportService reportService)
        {
            _reportService = reportService;
        }

        // Sayfaya öğrenci bilgisi geldiğinde çalışır
        async partial void OnStudentChanged(StudentDto value)
        {
            if (value != null)
            {
                await LoadAllReportsAsync();
            }
        }

        // Tarih seçimi değiştiğinde çalışır
        partial void OnSelectedDateChanged(DateTime value)
        {
            FilterReportByDate();
        }

        [RelayCommand]
        public async Task LoadAllReportsAsync()
        {
            if (IsBusy || Student == null) return;
            IsBusy = true;

            try
            {
                var token = Preferences.Get("AuthToken", string.Empty);
                // 1. Tüm raporları çek
                _allReports = await _reportService.GetStudentReportsAsync(token, Student.Id);

                // 2. Seçili tarihe (Bugüne) göre filtrele
                FilterReportByDate();
            }
            catch (Exception)
            {
                // Hata yönetimi
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

            // Seçilen tarihin gününe ait raporu bul
            var reportForDate = _allReports.FirstOrDefault(r => r.Date.Date == SelectedDate.Date);

            SetReport(reportForDate);
        }

        private void SetReport(DailyReportDto report)
        {
            CurrentReport = report;
            HasReport = report != null;
            IsReportEmpty = !HasReport; 

            // UI güncellemesi için notify tetikle
            OnPropertyChanged(nameof(MoodEmoji));
            OnPropertyChanged(nameof(MoodText));
            OnPropertyChanged(nameof(MoodColor));
            OnPropertyChanged(nameof(BreakfastText));
            OnPropertyChanged(nameof(LunchText));
            OnPropertyChanged(nameof(SleepText));
            OnPropertyChanged(nameof(ActivityText));
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            try
            {
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                try
                {
                    if (Shell.Current.Navigation.NavigationStack.Count > 0)
                    {
                        await Shell.Current.Navigation.PopAsync();
                    }
                }
                catch
                {
                    Console.WriteLine($"Geri gitme hatası: {ex.Message}");
                }
            }
        }
    }
}
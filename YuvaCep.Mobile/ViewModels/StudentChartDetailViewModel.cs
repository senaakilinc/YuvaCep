using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using YuvaCep.Mobile.Dtos;
using YuvaCep.Mobile.Enums;
using YuvaCep.Mobile.Services;

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

        // PUZZLE DEĞİŞKENLERİ
        [ObservableProperty] private string puzzleImage; // Ana Resim 
        [ObservableProperty] private bool isCompletedToday;
        [ObservableProperty] private string successMessage;

        // KOORDİNATLAR
        [ObservableProperty] private double holeX;       // Deliğin X konumu
        [ObservableProperty] private double holeY;       // Deliğin Y konumu
        [ObservableProperty] private double pieceImageX; // Parça resminin X kayması
        [ObservableProperty] private double pieceImageY; // Parça resminin Y kayması

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

                    var today = DateTime.Today;
                    IsCompletedToday = data.CompletedDates.Any(d => d.Date == today);

                    // Temaya göre resmi seç
                    var currentTheme = (ChartTheme)data.Theme;
                    SetupPuzzle(currentTheme, IsCompletedToday);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", ex.Message, "Tamam");
            }
            finally { IsBusy = false; }
        }

        private void SetupPuzzle(ChartTheme theme, bool isDone)
        {
            string imageName = "puzzle_default";

            switch (theme)
            {
                case ChartTheme.Hygiene: imageName = "puzzle_hygiene"; break; 
                case ChartTheme.Sport: imageName = "puzzle_sport"; break;     
                case ChartTheme.Education: imageName = "puzzle_reading"; break; 
                case ChartTheme.Nutrition: imageName = "puzzle_nutrition"; break;
                case ChartTheme.Art: imageName = "puzzle_art"; break;
                default: imageName = "puzzle_default"; break;
            }

            PuzzleImage = imageName;

            if (isDone)
            {
                SuccessMessage = "Harika İş Çıkardın! 🎉";
            }
            else
            {
                SuccessMessage = "";
                RandomizePuzzlePosition();
            }
        }

        // RASTGELE KONUM BELİRLEME
        private void RandomizePuzzlePosition()
        {
            var rnd = new Random();
            double x = rnd.Next(0, 201);
            double y = rnd.Next(0, 201);

            // Deliği bu konuma koy
            HoleX = x;
            HoleY = y;

            // Parçadaki resmi tam tersine kaydır ki eşleşsin
            PieceImageX = -x;
            PieceImageY = -y;
        }

        [RelayCommand]
        public async Task CompleteActivityAsync()
        {
            if (IsCompletedToday) return;

            IsBusy = true;
            try
            {
                var token = Preferences.Get("AuthToken", "");

                // Servisi çağırır
                var response = await _activityService.CompleteChartAsync(token, ChartDetail.Id, ChartDetail.StudentId, DateTime.Now);

                if (response.Success) 
                {
                    IsCompletedToday = true;

                    var currentTheme = (ChartTheme)ChartDetail.Theme;
                    SetupPuzzle(currentTheme, true);

                    var todayItem = Days.FirstOrDefault(d => d.Date.Date == DateTime.Today);
                    if (todayItem != null)
                    {
                        todayItem.IsCompleted = true;
                        todayItem.BoxColor = Colors.Green;
                        todayItem.TextColor = Colors.White;
                    }

                    // ROZET KONTROLÜ 
                    if (response.NewBadge != null)
                    {
                        var popup = new Views.Popups.BadgeWonPopup(response.NewBadge);
                        await Shell.Current.CurrentPage.ShowPopupAsync(popup);
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hata", "Kaydedilemedi: " + response.Message, "Tamam");
                }
            }
            finally { IsBusy = false; }
        }

        private void GenerateCalendar(ActivityChartDetailDto data)
        {
            Days.Clear();
            int daysInMonth = DateTime.DaysInMonth(data.Year, data.Month);
            DateTime today = DateTime.Today;

            for (int i = 1; i <= daysInMonth; i++)
            {
                DateTime currentDayDate = new DateTime(data.Year, data.Month, i);
                bool isDone = data.CompletedDates.Any(d => d.Date == currentDayDate.Date);

                Color boxColor = isDone ? Colors.Green : (currentDayDate < today ? Colors.Red : Color.FromArgb("#E0E0E0"));
                Color textColor = isDone || currentDayDate < today ? Colors.White : Colors.Black;

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

        [RelayCommand]
        private async Task GoBackAsync() => await Shell.Current.GoToAsync("..");
    }

    public class StudentDayModel : ObservableObject
    {
        public int DayNumber { get; set; }
        public DateTime Date { get; set; }

        private bool isCompleted;
        public bool IsCompleted { get => isCompleted; set => SetProperty(ref isCompleted, value); }

        private Color boxColor;
        public Color BoxColor { get => boxColor; set => SetProperty(ref boxColor, value); }

        private Color textColor;
        public Color TextColor { get => textColor; set => SetProperty(ref textColor, value); }
    }
}
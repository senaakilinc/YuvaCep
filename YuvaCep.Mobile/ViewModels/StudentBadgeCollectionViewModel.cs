using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace YuvaCep.Mobile.ViewModels
{
    public class BadgeDisplayItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsEarned { get; set; }
        public DateTime? EarnedDate { get; set; }

        public double Opacity => IsEarned ? 1.0 : 0.5;
        public string StatusText => IsEarned ? $"{EarnedDate:MMMM yyyy}" : "Kilitli 🔒";
        public Color CardColor => IsEarned ? Colors.White : Color.FromArgb("#F3F4F6");
    }

    [QueryProperty(nameof(StudentId), "StudentId")]
    public partial class StudentBadgeCollectionViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;

        [ObservableProperty] private string studentId;
        public ObservableCollection<BadgeDisplayItem> Badges { get; } = new();

        public StudentBadgeCollectionViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://10.0.2.2:5000") };
        }

        async partial void OnStudentIdChanged(string value)
        {
            if (!string.IsNullOrEmpty(value)) await LoadBadges();
        }

        private async Task LoadBadges()
        {
            try
            {
                Badges.Clear();
                var token = Preferences.Get("AuthToken", "");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"api/Gamification/student-badges?studentId={StudentId}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<List<BadgeDisplayItem>>();
                    if (data != null) foreach (var item in data) Badges.Add(item);
                }
            }
            catch (Exception ex)
            {
                // Hata yönetimi
            }
        }

        [RelayCommand]
        private async Task GoBackAsync() => await Shell.Current.GoToAsync("..");
    }
}
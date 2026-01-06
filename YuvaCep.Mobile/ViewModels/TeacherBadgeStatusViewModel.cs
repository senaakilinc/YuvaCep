using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace YuvaCep.Mobile.ViewModels
{
    public class ClassBadgeStatusItem
    {
        public Guid StudentId { get; set; }
        public string Name { get; set; }
        public int CompletedDays { get; set; }
        public string BadgeIcon { get; set; }
        public bool HasBadge { get; set; }
    }

    public partial class TeacherBadgeStatusViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;

        public ObservableCollection<ClassBadgeStatusItem> Students { get; } = new();

        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private string currentMonthYear = DateTime.Now.ToString("MMMM yyyy");

        public TeacherBadgeStatusViewModel()
        {

            _httpClient = new HttpClient { BaseAddress = new Uri("http://10.0.2.2:5000") };
            LoadClassData();
        }

        private async void LoadClassData()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                Students.Clear();
                var token = Preferences.Get("AuthToken", "");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("api/Gamification/class-status");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<List<ClassBadgeStatusItem>>();
                    if (data != null)
                    {
                        foreach (var item in data) Students.Add(item);
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
        private async Task GoBackAsync() => await Shell.Current.GoToAsync("..");
    }
}
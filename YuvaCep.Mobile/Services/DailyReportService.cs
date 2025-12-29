using System.Net.Http.Json;
using YuvaCep.Mobile.Dtos;

namespace YuvaCep.Mobile.Services
{
    public class DailyReportService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://10.0.2.2:5000";

        public DailyReportService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<List<DailyReportDto>> GetReportsAsync(string token, Guid studentId)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                // API: api/DailyReports/student/{studentId}
                var response = await _httpClient.GetAsync($"/api/DailyReports/student/{studentId}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<List<DailyReportDto>>();
                    return data ?? new List<DailyReportDto>();
                }
            }
            catch
            {
                return new List<DailyReportDto>();
            }
            return new List<DailyReportDto>();
        }

        // Bugün raporu girilmiş öğrenci ID'lerini getirir
        public async Task<List<Guid>> GetReportedStudentIdsAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.GetAsync("/api/DailyReports/reported-today-ids");

                if (response.IsSuccessStatusCode)
                {
                    var ids = await response.Content.ReadFromJsonAsync<List<Guid>>();
                    return ids ?? new List<Guid>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }

            return new List<Guid>();
        }

        public async Task<bool> AddReportAsync(string token, DailyReportDto report)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                var jsonPaketi = new
                {
                    StudentId = report.Id, // Burası Backend'de "StudentId" diye gidecek
                    Mood = (int)report.Mood,     
                    MoodNote = report.MoodNote ?? "",
                    Breakfast = (int)report.Breakfast,
                    Lunch = (int)report.Lunch,
                    FoodNote = report.FoodNote ?? "",
                    Sleep = (int)report.Sleep,
                    Activity = (int)report.Activity,
                    ActivityNote = report.ActivityNote ?? "",
                    TeacherNote = report.TeacherNote ?? ""
                };
             
                var response = await _httpClient.PostAsJsonAsync("/api/DailyReports", jsonPaketi);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    await Shell.Current.DisplayAlert("API Hatası",
                        $"Kod: {response.StatusCode}\nDetay: {errorBody}", "Tamam");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Bağlantı Hatası", ex.Message, "Tamam");
                return false;
            }
        }

        // Öğrencinin geçmiş tüm raporlarını getirir
        public async Task<List<DailyReportDto>> GetStudentReportsAsync(string token, Guid studentId)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.GetAsync($"/api/DailyReports/student/{studentId}");

                if (response.IsSuccessStatusCode)
                {
                    var reports = await response.Content.ReadFromJsonAsync<List<DailyReportDto>>();
                    return reports ?? new List<DailyReportDto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }

            return new List<DailyReportDto>();
        }

    }
}
using System.Net.Http.Json;
using YuvaCep.Mobile.Dtos;
using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Services
{
    public class ActivityService
    {
        private readonly HttpClient _httpClient;

        public ActivityService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Constants.BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(20);
        }

        public async Task<ActivityChartDetailDto> GetTeacherChartDetailsAsync(string token, Guid chartId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            try
            {
                var response = await _httpClient.GetAsync($"/api/ActivityCharts/{chartId}/details");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ActivityChartDetailDto>();
                }
            }
            catch { }
            return null;
        }

        public async Task<ActivityChartDetailDto> GetStudentChartDetailsAsync(string token, Guid chartId, Guid studentId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            try
            {
                string url = $"/api/ActivityCharts/{chartId}/student-details?studentId={studentId}";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ActivityChartDetailDto>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GetStudentChartDetailsAsync] Hata: {ex.Message}");
            }
            return null;
        }


        public async Task<bool> CreateActivityChartAsync(string token, string title, int themeValue, int month, int year)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            try
            {
                var model = new { Title = title, Theme = themeValue, Month = month, Year = year };
                var response = await _httpClient.PostAsJsonAsync("/api/ActivityCharts", model);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<List<ActivityChartDto>> GetTeacherChartsAsync(string token, int month, int year)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            try
            {
                var response = await _httpClient.GetAsync($"/api/ActivityCharts/teacher?month={month}&year={year}");
                return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<List<ActivityChartDto>>() : new List<ActivityChartDto>();
            }
            catch { return new List<ActivityChartDto>(); }
        }

        public async Task<List<ActivityChartDto>> GetStudentChartsAsync(string token, string studentId, int month, int year)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            try
            {
                var response = await _httpClient.GetAsync($"/api/ActivityCharts?studentId={studentId}&month={month}&year={year}");
                return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<List<ActivityChartDto>>() : new List<ActivityChartDto>();
            }
            catch { return new List<ActivityChartDto>(); }
        }

        public async Task<CompleteActivityResponseDto> CompleteChartAsync(string token, Guid chartId, Guid studentId, DateTime date)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var model = new { ActivityChartId = chartId, StudentId = studentId, Date = date };
                var response = await _httpClient.PostAsJsonAsync("api/ActivityCharts/complete", model);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CompleteActivityResponseDto>();
                }
            }
            catch (Exception ex)
            {
                // Hata logu gelebilir
            }

            return new CompleteActivityResponseDto { Success = false, Message = "Bağlantı hatası" };
        }

        public async Task<bool> DeleteChartAsync(string token, Guid chartId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"/api/ActivityCharts/{chartId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<BadgeDisplayItem>> GetStudentBadgesAsync(string token, string studentId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"/api/Gamification/student-badges?studentId={studentId}");
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<List<BadgeDisplayItem>>() : new List<BadgeDisplayItem>();
        }

        public async Task<List<ClassBadgeStatusItem>> GetClassBadgeStatusAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("/api/Gamification/class-status");
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<List<ClassBadgeStatusItem>>() : new List<ClassBadgeStatusItem>();
        }
    }
}
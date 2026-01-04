using System.Net.Http.Json;
using YuvaCep.Mobile.Dtos;

namespace YuvaCep.Mobile.Services
{
    public class AnnouncementService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://10.0.2.2:5000";

        public AnnouncementService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<List<AnnouncementDto>> GetAnnouncementsByStudentAsync(string token, Guid studentId)
        {
            SetToken(token);
            try
            {
                var response = await _httpClient.GetAsync($"/api/Announcements/student/{studentId}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<List<AnnouncementDto>>();
                    return data ?? new List<AnnouncementDto>();
                }
            }
            catch { }
            return new List<AnnouncementDto>();
        }

        public async Task<List<AnnouncementDto>> GetTeacherAnnouncementsAsync(string token)
        {
            SetToken(token);
            try
            {
                var response = await _httpClient.GetAsync("/api/Announcements/my-class");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<List<AnnouncementDto>>();
                    return data ?? new List<AnnouncementDto>();
                }
            }
            catch { }
            return new List<AnnouncementDto>();
        }

        public async Task<bool> CreateAnnouncementAsync(string token, string title, string content)
        {
            SetToken(token);
            try
            {
                var request = new { Title = title, Content = content, ClassId = (Guid?)null };
                var response = await _httpClient.PostAsJsonAsync("/api/Announcements", request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        private void SetToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}
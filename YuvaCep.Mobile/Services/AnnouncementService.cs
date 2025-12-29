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

        public async Task<List<AnnouncementDto>> GetAnnouncementsAsync(string token, Guid classId)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                // API endpointimiz: api/Announcements/class/{classId}
                var response = await _httpClient.GetAsync($"/api/Announcements/class/{classId}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<List<AnnouncementDto>>();
                    return data ?? new List<AnnouncementDto>();
                }
            }
            catch
            {
                // Hata olursa boş liste dön
                return new List<AnnouncementDto>();
            }
            return new List<AnnouncementDto>();
        }
    }
}
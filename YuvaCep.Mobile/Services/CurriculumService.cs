using System.Net.Http.Json;
using YuvaCep.Mobile.Dtos;

namespace YuvaCep.Mobile.Services
{
    public class CurriculumService
    {
        private readonly HttpClient _httpClient;

        public CurriculumService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Constants.BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(60);
        }

        public async Task<LessonProgramDto> GetProgramAsync(string token, int month, int year, string studentId)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                string url = $"/api/LessonPrograms?month={month}&year={year}";
                if (!string.IsNullOrEmpty(studentId)) url += $"&studentId={studentId}";

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return null;
                    return await response.Content.ReadFromJsonAsync<LessonProgramDto>();
                }
            }
            catch { }
            return null;
        }

        public async Task<bool> UploadProgramAsync(string token, DateTime date, List<string> images)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                var request = new { Date = date, Images = images };
                var response = await _httpClient.PostAsJsonAsync("/api/LessonPrograms", request);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteImageAsync(string token, Guid imageId)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.DeleteAsync($"/api/LessonPrograms/image/{imageId}");
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }
    }
}
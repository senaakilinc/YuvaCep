using System.Net.Http.Json;
using YuvaCep.Mobile.Dtos;

namespace YuvaCep.Mobile.Services
{
    public class FoodListService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://10.0.2.2:5000";

        public FoodListService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<FoodListDto> GetFoodListAsync(string token, int month, int year, string studentId)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                string url = $"/api/FoodLists?month={month}&year={year}";
                if (!string.IsNullOrEmpty(studentId))
                {
                    url += $"&studentId={studentId}";
                }

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return null;

                    var data = await response.Content.ReadFromJsonAsync<FoodListDto>();
                    return data;
                }
            }
            catch { }
            return null;
        }

        public async Task<bool> UploadFoodListAsync(string token, DateTime date, string base64Image)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                var request = new { Date = date, ImageBase64 = base64Image };
                var response = await _httpClient.PostAsJsonAsync("/api/FoodLists", request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
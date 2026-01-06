using System.Net.Http.Json;
using YuvaCep.Mobile.Dtos;

namespace YuvaCep.Mobile.Services
{
    public class ApiResponse
    {
        public string Message { get; set; }
    }
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Constants.BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<StudentDto> GetMyStudentAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.GetAsync("/api/Parent/my-student");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return null;
                    var options = new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    return await response.Content.ReadFromJsonAsync<StudentDto>(options);
                    
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        public async Task<(bool isSuccess, string message)> LinkStudentAsync(string token, string referenceCode)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new { ReferenceCode = referenceCode };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/Parent/link-student", request);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Eşleşme Başarılı!");
                }
                else
                {
                    var errorResult = await response.Content.ReadFromJsonAsync<ApiResponse>();
                    return (false, errorResult?.Message ?? "Bir hata oluştu.");
                }
            }
            catch (Exception ex)
            {
                return (false, "Bağlantı Hatası: " + ex.Message);
            }
        }
    }
}
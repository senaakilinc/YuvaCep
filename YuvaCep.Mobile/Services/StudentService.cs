using System.Net.Http.Json;
using YuvaCep.Mobile.Dtos;

namespace YuvaCep.Mobile.Services
{
    public class StudentService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://10.0.2.2:5000";

        public StudentService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        // Tüm öğrencileri getiren metod
        public async Task<List<StudentDto>> GetAllStudentsAsync(string token)
        {
            try
            {
                // Token'ı başlığa ekle
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // API'ye istek 
                var response = await _httpClient.GetAsync("/api/Students");

                if (response.IsSuccessStatusCode)
                {
                    var students = await response.Content.ReadFromJsonAsync<List<StudentDto>>();
                    return students ?? new List<StudentDto>();
                }
            }
            catch (Exception ex)
            {
                // Hata olursa boş liste dön
                Console.WriteLine($"Hata: {ex.Message}");
            }

            return new List<StudentDto>();
        }
    }
}
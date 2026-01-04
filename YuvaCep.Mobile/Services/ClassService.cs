using System.Net.Http.Json;
using YuvaCep.Mobile.Dtos;

namespace YuvaCep.Mobile.Services
{
    public class ClassService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://10.0.2.2:5000";

        public ClassService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<bool> CreateClassAsync(string className, string ageGroup)
        {
            try
            {
                // Token'ı al (Çünkü sadece kayıt olan öğretmen sınıf açabilir)
                var token = Preferences.Get("AuthToken", string.Empty);
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Veriyi paketle
                var request = new CreateClassRequest
                {
                    ClassName = className,
                    AgeGroup = ageGroup
                };

                // API'ye gönder
                var response = await _httpClient.PostAsJsonAsync("/api/Classes/create", request);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
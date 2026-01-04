using System.Net.Http.Json;
using YuvaCep.Mobile.Dtos;
using System.Text.Json;
using Microsoft.Maui.Storage;

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
            _httpClient.Timeout = TimeSpan.FromMinutes(1);
        }

        private async Task AddAuthorizationHeaderAsync()
        {
            var token = await SecureStorage.GetAsync("auth_token");

            if (string.IsNullOrEmpty(token))
                token = Preferences.Get("AuthToken", string.Empty);

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<StudentListDto>> GetMyStudentsAsync()
        {
            try
            {
                await AddAuthorizationHeaderAsync();

                var jsonString = await _httpClient.GetStringAsync("/api/Students/my-class-students");

                System.Diagnostics.Debug.WriteLine($"API CEVABI (MyStudents): {jsonString}");

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<List<StudentListDto>>(jsonString, options);

                return result ?? new List<StudentListDto>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LİSTE ÇEKME HATASI: {ex.Message}");
                return new List<StudentListDto>();
            }
        }

        public async Task<string> AddStudentAsync(string name, string surname)
        {
            try
            {
                await AddAuthorizationHeaderAsync();

                var request = new CreateStudentRequest
                {
                    Name = name,
                    Surname = surname
                };

                var response = await _httpClient.PostAsJsonAsync("/api/Students/add", request);

                if (response.IsSuccessStatusCode)
                {
                    return "OK";
                }

                var errorMsg = await response.Content.ReadAsStringAsync();
                return $"Sunucu Hatası ({response.StatusCode}): {errorMsg}";
            }
            catch (Exception ex)
            {
                return $"Bağlantı Hatası: {ex.Message}";
            }
        }

        public async Task<string> LinkStudentAsync(string code)
        {
            try
            {
                await AddAuthorizationHeaderAsync();
                var request = new LinkStudentRequest { ReferenceCode = code };

                System.Diagnostics.Debug.WriteLine($">>> API İSTEĞİ ATILIYOR: /api/Students/link-student | Kod: {code}");

                var response = await _httpClient.PostAsJsonAsync("/api/Students/link-student", request);

                if (response.IsSuccessStatusCode) return "OK";

                var errorMsg = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($">>> SUNUCU HATASI DÖNDÜ: {response.StatusCode} - {errorMsg}");
                return $"Sunucu Hatası: {errorMsg}";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"KRİTİK HATA OLUŞTU: {ex.Message}");
                return $"Uygulama Hatası: {ex.Message}";
            }
        }

        public async Task<List<ChildDto>> GetMyChildrenAsync()
        {
            try
            {
                await AddAuthorizationHeaderAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return await _httpClient.GetFromJsonAsync<List<ChildDto>>("/api/Students/my-children", options);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ÇOCUKLARI ÇEKME HATASI: {ex.Message}");
                return new List<ChildDto>();
            }
        }

        // --- İYİLEŞTİRİLMİŞ DETAY METODU ---
        public async Task<StudentDetailDto> GetStudentDetailAsync(Guid id)
        {
            await AddAuthorizationHeaderAsync();

            try
            {
                // Önce string olarak çekiyoruz ki loglayabilelim
                var response = await _httpClient.GetAsync($"api/students/{id}/detail");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    // BU SATIR ÇOK ÖNEMLİ: Output penceresinde gelen veriyi göreceksin
                    System.Diagnostics.Debug.WriteLine($"--------------------------------------------------");
                    System.Diagnostics.Debug.WriteLine($"DETAY JSON CEVABI: {jsonString}");
                    System.Diagnostics.Debug.WriteLine($"--------------------------------------------------");

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<StudentDetailDto>(jsonString, options);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"DETAY ÇEKİLEMEDİ: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Detay çekme hatası: " + ex.Message);
            }
            return null;
        }

        // --- İYİLEŞTİRİLMİŞ GÜNCELLEME METODU ---
        public async Task<bool> UpdateStudentAsync(StudentUpdateDto model)
        {
            await AddAuthorizationHeaderAsync();
            try
            {
                // Ne gönderdiğimizi görelim
                System.Diagnostics.Debug.WriteLine($"GÜNCELLEME ID: {model.Id} | İSİM: {model.FirstName} {model.LastName}");

                var response = await _httpClient.PutAsJsonAsync($"api/students/{model.Id}", model);

                if (!response.IsSuccessStatusCode)
                {
                    // Hata varsa nedenini oku ve yazdır
                    var errorBody = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"GÜNCELLEME BAŞARISIZ! Kod: {response.StatusCode}");
                    System.Diagnostics.Debug.WriteLine($"HATA DETAYI: {errorBody}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GÜNCELLEME EXCEPTION: {ex.Message}");
                return false;
            }
        }
    }
}
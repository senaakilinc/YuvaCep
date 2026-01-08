using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage; 
using YuvaCep.Mobile.Dtos;

namespace YuvaCep.Mobile.Services
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public string UserRole { get; set; }
        public string Message { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public string ClassName { get; set; } 
        public Guid? ClassId { get; set; }    
    }

    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;


        public AuthService()
        {

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Constants.BaseUrl);
            _httpClient.Timeout = TimeSpan.FromMinutes(2);

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Büyük/Küçük harf uyumu sağlar
            };
        }

        // GİRİŞ YAPMA 
        public async Task<LoginResponse> LoginAsync(string tcidnumber, string password)
        {
            var loginData = new { TCIDNumber = tcidnumber, Password = password };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Auth/login", loginData);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>(_serializerOptions);

                    if (result != null)
                    {
                        result.IsSuccess = true;

                        // Token'ı kaydet
                        await SecureStorage.SetAsync("auth_token", result.Token);

                        // Kullanıcı Adını Kaydet 
                        string fullName = $"{result.Name} {result.Surname}";
                        Preferences.Set("UserName", fullName);

                        // Rolü Kaydet
                        Preferences.Set("UserRole", result.UserRole);

                        // SINIF BİLGİSİNİ KAYDET
                        if (!string.IsNullOrEmpty(result.ClassName))
                        {
                            Preferences.Set("ClassName", result.ClassName);
                            Preferences.Set("ClassId", result.ClassId.ToString());
                        }
                        else
                        {
                            // Eğer sınıfı yoksa eski veriyi temizle
                            Preferences.Remove("ClassName");
                            Preferences.Remove("ClassId");
                        }

                    }
                    return result;
                }
                else
                {
                    var errorResult = await response.Content.ReadFromJsonAsync<LoginResponse>(_serializerOptions);
                    return errorResult ?? new LoginResponse { IsSuccess = false, Message = "Giriş başarısız." };
                }
            }
            catch (Exception ex)
            {
                return new LoginResponse { IsSuccess = false, Message = $"Bağlantı hatası: {ex.Message}" };
            }
        }

        // VELİ KAYIT 
        public async Task<LoginResponse> RegisterParentAsync(ParentRegisterRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Auth/register/parent", request);
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>(_serializerOptions);

                if (result == null) return new LoginResponse { IsSuccess = false, Message = "Sunucudan boş cevap döndü." };

                // Kayıt başarılıysa otomatik giriş yapılmış gibi verileri kaydet
                if (!string.IsNullOrEmpty(result.Token))
                {
                    await SecureStorage.SetAsync("auth_token", result.Token);
                    Preferences.Set("UserRole", "Parent");
                    Preferences.Set("UserName", $"{result.Name} {result.Surname}");
                }

                return result;
            }
            catch (Exception ex)
            {
                return new LoginResponse { IsSuccess = false, Message = $"Bağlantı hatası: {ex.Message}" };
            }
        }

        // ÖĞRETMEN KAYIT 
        public async Task<LoginResponse> RegisterTeacherAsync(TeacherRegisterRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Auth/register/teacher", request);
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>(_serializerOptions);

                if (result == null) return new LoginResponse { IsSuccess = false, Message = "Sunucudan boş cevap döndü." };

                if (!string.IsNullOrEmpty(result.Token))
                {
                    await SecureStorage.SetAsync("auth_token", result.Token);
                    Preferences.Set("UserRole", "Teacher");
                    Preferences.Set("UserName", $"{result.Name} {result.Surname}");
                }

                return result;
            }
            catch (Exception ex)
            {
                return new LoginResponse { IsSuccess = false, Message = $"Bağlantı hatası: {ex.Message}" };
            }
        }
    }
}
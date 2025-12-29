using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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
    }

    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _serializerOptions;

        private const string ApiUrl = "http://10.0.2.2:5000";

        public AuthService()
        {
            _baseUrl = ApiUrl;

            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromMinutes(2);

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Büyük/Küçük harf hatasını önler
            };
        }

        // GİRİŞ YAPMA FONKSİYONU
        public async Task<LoginResponse> LoginAsync(string tcidnumber, string password)
        {
            var loginData = new { TCIDNumber = tcidnumber, Password = password };

            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/Auth/login", loginData);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>(_serializerOptions);

                    if (result != null)
                    {
                        result.IsSuccess = true; // HTTP 200 geldiyse başarılıdır
                    }

                    return result;
                }
                else
                {
                    // Hata mesajını okumaya çalışalım
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return new LoginResponse { IsSuccess = false, Message = "Giriş başarısız. Bilgileri kontrol ediniz." };
                }
            }
            catch (Exception ex)
            {
                return new LoginResponse { IsSuccess = false, Message = $"Bağlantı hatası: {ex.Message}" };
            }
        }

        // KAYIT FONKSİYONU
        public async Task<bool> RegisterAsync(string tcidnumber, string password, string name, string surname, string userType)
        {
            var registerData = new
            {
                TCIDNumber = tcidnumber,
                Password = password,
                Name = name,
                Surname = surname
            };

            string endpointUrl;

            // Kullanıcı tipine göre doğru adresi seçiyoruz
            if (userType == "Öğretmen" || userType == "Teacher")
            {
                endpointUrl = $"{_baseUrl}/api/Auth/register/teacher";
            }
            else
            {
                endpointUrl = $"{_baseUrl}/api/Auth/register/parent";
            }

            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpointUrl, registerData);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
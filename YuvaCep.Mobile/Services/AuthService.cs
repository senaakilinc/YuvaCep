using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using YuvaCep.Mobile.Dtos;

namespace YuvaCep.Mobile.Services
{
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
                PropertyNameCaseInsensitive = true
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
                    return result;
                }
                else
                {
                    return new LoginResponse { IsSuccess = false, Message = "Sunucu hatası veya hatalı giriş." };
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
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using YuvaCep.Mobile.Dtos;
using YuvaCep.Mobile.Dtos.YuvaCep.Mobile.Dtos;


namespace YuvaCep.Mobile.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _serializerOptions;

        public AuthService()
        {
            _httpClient = new HttpClient();
            _baseUrl = Constants.BaseUrl;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        // GİRİŞ YAPMA FONKSİYONU
        public async Task<LoginResponse> LoginAsync(string tcidnumber,string password)
        {
            var loginData = new {TCIDNumber = tcidnumber, Password = password};

            try
            {   
                //API'ye POST isteği
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/Auth/login", loginData);
                
                if (response.IsSuccessStatusCode)
                {
                    //Gelen Json u nesneye çevirir.
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>(_serializerOptions);
                    return result;
                }
                else
                {
                    //Hata varsa döner.
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
                Surname = surname,
                Role = userType == "Öğretmen" ? 1 : 2
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/Auth/register", registerData);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false; 
            }
        }
    }
}

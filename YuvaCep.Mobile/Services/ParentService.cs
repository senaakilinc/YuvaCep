using System.Net.Http.Json;
using System.Net.Http.Headers;
using YuvaCep.Mobile.Dtos;

namespace YuvaCep.Mobile.Services
{
    public class ParentService
    {
        private readonly HttpClient _httpClient;

        public ParentService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Constants.BaseUrl);
            _httpClient.Timeout = TimeSpan.FromMinutes(1);
        }

        private async Task AddAuthorizationHeaderAsync()
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<bool> UpdateContactInfoAsync(string newPhoneNumber)
        {
            try
            {
                await AddAuthorizationHeaderAsync(); // Token ekle

                var model = new { PhoneNumber = newPhoneNumber };

                var response = await _httpClient.PostAsJsonAsync("api/Parent/update-contact", model);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Hata: {ex.Message}");
                return false;
            }
        }

        public async Task<ParentProfileDto> GetMyProfileAsync()
        {
            try
            {
                await AddAuthorizationHeaderAsync();
                var response = await _httpClient.GetAsync("api/Parent/my-profile");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ParentProfileDto>();
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Profil Çekme Hatası: {ex.Message}");
                return null;
            }
        }

        public async Task<(bool IsSuccess, string Message)> ChangePasswordAsync(string currentPass, string newPass)
        {
            try
            {
                await AddAuthorizationHeaderAsync(); 

                var model = new { CurrentPassword = currentPass, NewPassword = newPass };

                var response = await _httpClient.PostAsJsonAsync("api/Parent/change-password", model);

                if (response.IsSuccessStatusCode)
                {
                    return (true, "Şifre başarıyla değiştirildi.");
                }
                else
                {
                    var errorString = await response.Content.ReadAsStringAsync();
                    return (false, "İşlem başarısız. Mevcut şifrenizi kontrol ediniz.");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Bağlantı hatası: {ex.Message}");
            }
        }
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using YuvaCep.Mobile.Services;

namespace YuvaCep.Mobile.ViewModels
{
    [QueryProperty(nameof(StudentId), "studentId")]
    public partial class FoodListViewModel : ObservableObject
    {
        private readonly FoodListService _foodListService;

        [ObservableProperty] private string studentId;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private ImageSource currentImage; 
        [ObservableProperty] private bool hasImage;
        [ObservableProperty] private bool isImageEmpty;
        [ObservableProperty] private DateTime selectedDate = DateTime.Now;

        public bool IsTeacher => Preferences.Get("UserRole", "") == "Teacher";

        public FoodListViewModel(FoodListService foodListService)
        {
            _foodListService = foodListService;
        }

        partial void OnStudentIdChanged(string value)
        {
            MainThread.BeginInvokeOnMainThread(async () => await LoadDataAsync());
        }

        partial void OnSelectedDateChanged(DateTime value)
        {
            MainThread.BeginInvokeOnMainThread(async () => await LoadDataAsync());
        }

        [RelayCommand]
        public async Task LoadDataAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var token = Preferences.Get("AuthToken", string.Empty);
                var data = await _foodListService.GetFoodListAsync(token, SelectedDate.Month, SelectedDate.Year, StudentId);

                if (data != null && !string.IsNullOrEmpty(data.ImageBase64))
                {

                    byte[] imageBytes = Convert.FromBase64String(data.ImageBase64);
                    CurrentImage = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    HasImage = true;
                    IsImageEmpty = false;
                }
                else
                {
                    CurrentImage = null;
                    HasImage = false;
                    IsImageEmpty = true;
                }
            }
            catch
            {
                await Shell.Current.DisplayAlert("Hata", "Yemek listesi yüklenemedi.", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task PickAndUploadImageAsync()
        {
            if (!IsTeacher) return;

            try
            {
                var result = await MediaPicker.Default.PickPhotoAsync();
                if (result != null)
                {
                    IsBusy = true;

                    using var stream = await result.OpenReadAsync();
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);

                    string base64 = Convert.ToBase64String(memoryStream.ToArray());

                    var token = Preferences.Get("AuthToken", string.Empty);
                    bool success = await _foodListService.UploadFoodListAsync(token, SelectedDate, base64);

                    IsBusy = false;

                    if (success)
                    {
                        await Shell.Current.DisplayAlert("Başarılı", "Yemek listesi güncellendi.", "Tamam");
                        await LoadDataAsync();
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Hata", "Yükleme başarısız.", "Tamam");
                    }
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Hata", $"Resim seçilemedi: {ex.Message}", "Tamam");
            }
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
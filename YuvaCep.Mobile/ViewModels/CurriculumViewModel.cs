using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using YuvaCep.Mobile.Services;

#if ANDROID
using Android.Graphics;
#endif

namespace YuvaCep.Mobile.ViewModels
{
    // Resim Wrapper Sınıfı
    public partial class CurriculumImageItem : ObservableObject
    {
        public Guid Id { get; set; }
        public ImageSource Image { get; set; }
        public bool IsTeacher { get; set; }

        private readonly Action<CurriculumImageItem> _deleteAction;

        public CurriculumImageItem(Action<CurriculumImageItem> deleteAction)
        {
            _deleteAction = deleteAction;
        }

        [RelayCommand]
        private void Delete()
        {
            _deleteAction?.Invoke(this);
        }
    }

    [QueryProperty(nameof(StudentId), "studentId")]
    public partial class CurriculumViewModel : ObservableObject
    {
        private readonly CurriculumService _curriculumService;

        [ObservableProperty] private string studentId;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private bool hasImages;
        [ObservableProperty] private bool isImageEmpty;
        [ObservableProperty] private DateTime selectedDate = DateTime.Now;

        public ObservableCollection<CurriculumImageItem> CarouselImages { get; } = new();

        public bool IsTeacher => Preferences.Get("UserRole", "") == "Teacher" && string.IsNullOrEmpty(StudentId);

        public CurriculumViewModel(CurriculumService curriculumService)
        {
            _curriculumService = curriculumService;
        }

        partial void OnStudentIdChanged(string value) => MainThread.BeginInvokeOnMainThread(async () => await LoadDataAsync());
        partial void OnSelectedDateChanged(DateTime value) => MainThread.BeginInvokeOnMainThread(async () => await LoadDataAsync());

        [RelayCommand]
        public async Task LoadDataAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var token = Preferences.Get("AuthToken", string.Empty);
                var data = await _curriculumService.GetProgramAsync(token, SelectedDate.Month, SelectedDate.Year, StudentId);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    CarouselImages.Clear();

                    if (data != null && data.Images != null && data.Images.Any())
                    {
                        foreach (var item in data.Images)
                        {
                            byte[] imageBytes = Convert.FromBase64String(item.ImageBase64);
                            CarouselImages.Add(new CurriculumImageItem(async (i) => await DeleteImageAsync(i))
                            {
                                Id = item.Id,
                                Image = ImageSource.FromStream(() => new MemoryStream(imageBytes)),
                                IsTeacher = this.IsTeacher
                            });
                        }
                        HasImages = true;
                        IsImageEmpty = false;
                    }
                    else
                    {
                        HasImages = false;
                        IsImageEmpty = true;
                    }
                });
            }
            catch { }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task PickAndUploadImagesAsync()
        {
            if (!IsTeacher) return;

            try
            {
                var results = await FilePicker.Default.PickMultipleAsync(new PickOptions
                {
                    PickerTitle = "Resimleri Seçiniz",
                    FileTypes = FilePickerFileType.Images
                });

                if (results != null && results.Any())
                {

                    List<string> base64List = new();

                    foreach (var photo in results)
                    {
                        try
                        {
                            using var stream = await photo.OpenReadAsync();
                            string resizedBase64 = await ResizeAndCompressImageAsync(stream);

                            if (!string.IsNullOrEmpty(resizedBase64))
                            {
                                base64List.Add(resizedBase64);
                            }
                            GC.Collect();
                        }
                        catch (Exception innerEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"Hata: {innerEx.Message}");
                        }
                    }

                    if (base64List.Count > 0)
                    {
                        IsBusy = true;

                        var token = Preferences.Get("AuthToken", string.Empty);
                        bool success = await _curriculumService.UploadProgramAsync(token, SelectedDate, base64List);

                        IsBusy = false;

                        if (success)
                        {
                            await Shell.Current.DisplayAlert("Başarılı", "Resimler eklendi.", "Tamam");
                            await LoadDataAsync();
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Hata", "Sunucu yanıt vermedi.", "Tamam");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Hata", ex.Message, "Tamam");
            }
        }

        private async Task<string> ResizeAndCompressImageAsync(Stream originalStream)
        {
            string result = null;

#if ANDROID
            try
            {
                BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
                await BitmapFactory.DecodeStreamAsync(originalStream, null, options);

                originalStream.Position = 0;
                options.InJustDecodeBounds = false;
                options.InSampleSize = CalculateInSampleSize(options, 1024, 1024);

                var originalBitmap = await BitmapFactory.DecodeStreamAsync(originalStream, null, options);

                if (originalBitmap != null)
                {
                    float maxDimension = 1024f;
                    float ratio = Math.Min(maxDimension / originalBitmap.Width, maxDimension / originalBitmap.Height);

                    int newWidth = (int)(originalBitmap.Width * ratio);
                    int newHeight = (int)(originalBitmap.Height * ratio);

                    var resizedBitmap = Bitmap.CreateScaledBitmap(originalBitmap, newWidth, newHeight, true);

                    using var ms = new MemoryStream();
                    await resizedBitmap.CompressAsync(Bitmap.CompressFormat.Jpeg, 70, ms);
                    byte[] byteArray = ms.ToArray();
                    result = Convert.ToBase64String(byteArray);

                    originalBitmap.Recycle();
                    resizedBitmap.Recycle();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Native hata: {ex.Message}");
                result = null;
            }
#else
            try 
            {
                using var ms = new MemoryStream();
                await originalStream.CopyToAsync(ms);
                result = Convert.ToBase64String(ms.ToArray());
            }
            catch { result = null; }
#endif
            return result;
        }

#if ANDROID
        public int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            int height = options.OutHeight;
            int width = options.OutWidth;
            int inSampleSize = 1;

            if (height > reqHeight || width > reqWidth)
            {
                int halfHeight = height / 2;
                int halfWidth = width / 2;

                while ((halfHeight / inSampleSize) >= reqHeight && (halfWidth / inSampleSize) >= reqWidth)
                {
                    inSampleSize *= 2;
                }
            }
            return inSampleSize;
        }
#endif

        private async Task DeleteImageAsync(CurriculumImageItem item)
        {
            if (item == null) return;

            bool answer = await Shell.Current.DisplayAlert("Sil", "Bu resmi silmek istediğinize emin misiniz?", "Evet", "Hayır");
            if (!answer) return;

            IsBusy = true;
            try
            {
                var token = Preferences.Get("AuthToken", string.Empty);
                bool success = await _curriculumService.DeleteImageAsync(token, item.Id);

                if (success)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        CarouselImages.Remove(item);
                        if (CarouselImages.Count == 0)
                        {
                            HasImages = false;
                            IsImageEmpty = true;
                        }
                    });
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hata", "Silinemedi.", "Tamam");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
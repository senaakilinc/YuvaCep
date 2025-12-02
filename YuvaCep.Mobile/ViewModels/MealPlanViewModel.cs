using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Media;

namespace YuvaCep.Mobile.ViewModels
{

    public partial class MealPlanViewModel : ObservableObject
    {
        //Ekranda görünecek resimin kaynağı
        [ObservableProperty]
        private ImageSource menuImage;

        //Resim seçildi mi kontrolü
        [ObservableProperty]
        private bool isImageSelected = false;

        public MealPlanViewModel()
        {
            //Başlangıçta boş
            isImageSelected = false;
        }

        [RelayCommand]
        private async Task PickImageAsync()
        {
            try
            {
                // 1. Galeriden Resmi Seç
                var result = await MediaPicker.Default.PickPhotoAsync();

                if (result != null)
                {
                    // 2. Resmi geçici bir dosyaya kaydediyoruz.
                    var localFilePath = Path.Combine(FileSystem.CacheDirectory, result.FileName);

                    using var sourceStream = await result.OpenReadAsync();
                    using var localFileStream = File.OpenWrite(localFilePath);

                    await sourceStream.CopyToAsync(localFileStream);

                    //3. Resmi ekranda göster
                    MenuImage = ImageSource.FromFile(localFilePath);

                    // 4. Bayrağı aç
                    IsImageSelected = true;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Resim yüklenirken hata oluştu: {ex.Message}", "Tamam");
            }
        }

        [RelayCommand]
        private async Task SaveMenuAsync()
        {
            if (isImageSelected == false)
            {
                await Shell.Current.DisplayAlert("Uyarı", "Lütfen önce bir menü fotoğrafı yükleyiniz.", "Tamam");
                return;
            }

            //Database Kayıt Simülasyonu
            await Shell.Current.DisplayAlert("Başarılı", "Yemek listesi sisteme yüklendi!", "Tamam");

            //İşlem bitince ana sayfaya dön
            await Shell.Current.GoToAsync("..");

        }
    }

}

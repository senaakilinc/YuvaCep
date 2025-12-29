using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using YuvaCep.Mobile.Dtos;
using YuvaCep.Mobile.Services;

namespace YuvaCep.Mobile.ViewModels
{
    // Bir önceki sayfadan (Öğrenci Detay) gönderilen Öğrenciyi yakalar
    [QueryProperty(nameof(Student), "Student")]
    public partial class AnnouncementsViewModel : ObservableObject
    {
        private readonly AnnouncementService _announcementService;

        [ObservableProperty]
        private StudentDto student;

        [ObservableProperty]
        private bool isBusy;

        // Ekranda gösterilecek gerçek liste
        public ObservableCollection<AnnouncementDto> Announcements { get; } = new();

        public AnnouncementsViewModel(AnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        // Sayfa yüklenirken veya Öğrenci bilgisi geldiğinde çalışır
        async partial void OnStudentChanged(StudentDto value)
        {
            if (value != null)
            {
                await LoadAnnouncementsAsync();
            }
        }

        [RelayCommand]
        public async Task LoadAnnouncementsAsync()
        {
            if (IsBusy || Student == null) return;
            IsBusy = true;

            try
            {
                var token = Preferences.Get("AuthToken", string.Empty);

                // SERVİSTEN VERİYİ ÇEK (Sınıf ID'sine göre)
                var list = await _announcementService.GetAnnouncementsAsync(token, Student.ClassId);

                Announcements.Clear();
                foreach (var item in list)
                {
                    Announcements.Add(item);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", "Duyurular yüklenirken hata oluştu.", "Tamam");
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
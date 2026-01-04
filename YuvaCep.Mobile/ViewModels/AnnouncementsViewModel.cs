using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using YuvaCep.Mobile.Dtos;
using YuvaCep.Mobile.Services;

namespace YuvaCep.Mobile.ViewModels
{
    [QueryProperty(nameof(StudentId), "studentId")]
    public partial class AnnouncementsViewModel : ObservableObject
    {
        private readonly AnnouncementService _announcementService;

        [ObservableProperty]
        private string studentId;

        [ObservableProperty]
        private bool isBusy;

        public bool IsTeacher => Preferences.Get("UserRole", "") == "Teacher";

        public ObservableCollection<AnnouncementDto> Announcements { get; } = new();

        public AnnouncementsViewModel(AnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        partial void OnStudentIdChanged(string value)
        {
            if (!string.IsNullOrEmpty(value) && !IsTeacher)
            {
                MainThread.BeginInvokeOnMainThread(async () => await LoadAnnouncementsAsync());
            }
        }

        [RelayCommand]
        public async Task LoadAnnouncementsAsync()
        {

            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var token = Preferences.Get("AuthToken", string.Empty);
                List<AnnouncementDto> list = new();

                if (IsTeacher)
                {
                    list = await _announcementService.GetTeacherAnnouncementsAsync(token);
                }
                else
                {
                    if (Guid.TryParse(StudentId, out Guid guidId))
                    {
                        list = await _announcementService.GetAnnouncementsByStudentAsync(token, guidId);
                    }
                }

                Announcements.Clear();
                foreach (var item in list.OrderByDescending(x => x.CreatedDate))
                {
                    Announcements.Add(item);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Hata: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task AddAnnouncementAsync()
        {
            if (!IsTeacher) return;

            string title = await Shell.Current.DisplayPromptAsync("Yeni Duyuru", "Duyuru başlığını giriniz:");
            if (string.IsNullOrWhiteSpace(title)) return;

            string content = await Shell.Current.DisplayPromptAsync("Yeni Duyuru", "Duyuru içeriğini giriniz:");
            if (string.IsNullOrWhiteSpace(content)) return;

            bool answer = await Shell.Current.DisplayAlert("Duyuru Yayınla",
                "Duyuru tüm velilere bildirim olarak gidecektir. Yayınlamak istediğinize emin misiniz?",
                "Evet, Yayınla", "Vazgeç");

            if (!answer) return;

            IsBusy = true; 

            try
            {
                var token = Preferences.Get("AuthToken", string.Empty);
                bool success = await _announcementService.CreateAnnouncementAsync(token, title, content);

                IsBusy = false;

                if (success)
                {
                    await Shell.Current.DisplayAlert("Başarılı", "Duyuru tüm sınıfa gönderildi.", "Tamam");
                    await LoadAnnouncementsAsync();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hata", "Duyuru gönderilemedi.", "Tamam");
                }
            }
            catch (Exception ex)
            {
                IsBusy = false; 
                await Shell.Current.DisplayAlert("Hata", ex.Message, "Tamam");
            }
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
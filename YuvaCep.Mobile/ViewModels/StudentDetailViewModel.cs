using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using YuvaCep.Mobile.Dtos;
using YuvaCep.Mobile.Services;

namespace YuvaCep.Mobile.ViewModels
{
    public partial class StudentDetailViewModel : ObservableObject, IQueryAttributable
    {
        private readonly StudentService _service;
        private Guid _realStudentId;

        public bool IsTeacher => Preferences.Get("UserRole", "") == "Teacher";
        public bool IsParent => !IsTeacher;

        [ObservableProperty] string firstName;
        [ObservableProperty] string lastName;
        [ObservableProperty] string parentName;
        [ObservableProperty] string gender;
        [ObservableProperty] string tCIDNumber;
        [ObservableProperty] DateTime dateOfBirth = DateTime.Now;
        [ObservableProperty] string healthNotes;

        [ObservableProperty] ImageSource profileImage;
        [ObservableProperty] string _base64Photo;
        [ObservableProperty] string healthNotesPlaceholder;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotNameEditing))]
        bool isNameEditing = false;

        public bool IsNotNameEditing => !IsNameEditing;

        [ObservableProperty] bool isBusy;

        public StudentDetailViewModel()
        {
            _service = new StudentService();
            ProfileImage = "icon_student.png";
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("id"))
            {
                string idStr = query["id"].ToString();
                if (Guid.TryParse(idStr, out Guid guidId))
                {
                    _realStudentId = guidId;
                    LoadStudentData(_realStudentId);
                }
            }
        }

        private async void LoadStudentData(Guid id)
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var data = await _service.GetStudentDetailAsync(id);
                if (data != null)
                {
                    FirstName = data.FirstName;
                    LastName = data.LastName;

                    if (!string.IsNullOrEmpty(data.Gender))
                    {
                        Gender = data.Gender.Trim();
                    }

                    ParentName = data.ParentName;
                    TCIDNumber = data.TCIDNumber;
                    DateOfBirth = data.DateOfBirth ?? DateTime.Now;
                    HealthNotes = data.HealthNotes;

                    if (IsTeacher)
                    {
                        HealthNotesPlaceholder = string.IsNullOrEmpty(data.HealthNotes)
                            ? "Henüz eklenen sağlık notu yok."
                            : "";
                    }
                    else
                    {
                        HealthNotesPlaceholder = "Alerji, ilaç kullanımı vb. durumları giriniz...";
                    }

                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        void ToggleNameEdit()
        {
            // Sadece öğretmen isme tıklayınca edit modunu açabilir
            if (IsTeacher) IsNameEditing = !IsNameEditing;
        }

        [RelayCommand]
        async Task ChangePhoto()
        {
            // Sadece öğretmen fotoğraf değiştirebilir
            if (!IsTeacher) return;

            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                if (photo != null)
                {
                    var stream = await photo.OpenReadAsync();
                    ProfileImage = ImageSource.FromStream(() => stream);

                    using var memoryStream = new MemoryStream();
                    using var fileStream = await photo.OpenReadAsync();
                    await fileStream.CopyToAsync(memoryStream);
                    _base64Photo = Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            catch { }
        }

        [RelayCommand]
        async Task Save()
        {
            if (IsBusy) return;
            IsBusy = true;

            var updateModel = new StudentUpdateDto
            {
                Id = _realStudentId,
                FirstName = FirstName,
                LastName = LastName,
                Gender = Gender,
                TCIDNumber = TCIDNumber,
                DateOfBirth = DateOfBirth,
                HealthNotes = HealthNotes,
                PhotoBase64 = _base64Photo
            };

            bool success = await _service.UpdateStudentAsync(updateModel);

            if (success)
            {
                IsNameEditing = false;
                await Shell.Current.DisplayAlert("Başarılı", "Bilgiler güncellendi", "Tamam");

                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Hata", "Güncelleme başarısız.", "Tamam");
            }
            IsBusy = false;
        }
    }
}
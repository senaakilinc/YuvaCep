using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using YuvaCep.Mobile.Dtos;
using YuvaCep.Mobile.Services;
using YuvaCep.Mobile.Views;

namespace YuvaCep.Mobile.ViewModels
{
    public partial class StudentListViewModel : ObservableObject
    {
        private readonly StudentService _studentService;

        [ObservableProperty] private string className;
        [ObservableProperty] private bool isBusy;

        // --- POP-UP ---
        [ObservableProperty] private bool isPopupVisible;
        [ObservableProperty] private string newStudentName;
        [ObservableProperty] private string newStudentSurname;

        // Liste bu türden nesneler tutuyor:
        public ObservableCollection<StudentListDto> Students { get; } = new();

        public StudentListViewModel(StudentService studentService)
        {
            _studentService = studentService;
            ClassName = Preferences.Get("ClassName", "Sınıfım");
            LoadStudentsAsync();
        }

        [RelayCommand]
        public async Task LoadStudentsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                Students.Clear();
                var list = await _studentService.GetMyStudentsAsync();

                foreach (var student in list)
                {
                    Students.Add(student);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Liste yüklenirken hata: {ex.Message}", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private void OpenPopup()
        {
            NewStudentName = "";
            NewStudentSurname = "";
            IsPopupVisible = true;
        }

        [RelayCommand]
        private void ClosePopup()
        {
            IsPopupVisible = false;
        }

        [RelayCommand]
        private async Task SaveStudentAsync()
        {
            if (string.IsNullOrWhiteSpace(NewStudentName) || string.IsNullOrWhiteSpace(NewStudentSurname))
            {
                await Shell.Current.DisplayAlert("Uyarı", "Lütfen ad ve soyad giriniz.", "Tamam");
                return;
            }

            IsBusy = true;
            string result = await _studentService.AddStudentAsync(NewStudentName.Trim(), NewStudentSurname.Trim());
            IsBusy = false;

            if (result == "OK")
            {
                await Shell.Current.DisplayAlert("Başarılı", "Öğrenci sınıfa eklendi!", "Tamam");
                IsPopupVisible = false;
                NewStudentName = "";
                NewStudentSurname = "";
                await LoadStudentsAsync();
            }
            else
            {
                await Shell.Current.DisplayAlert("Hata Detayı", result, "Tamam");
            }
        }

        [RelayCommand]
        async Task GoToDetail(StudentListDto student)
        {
            if (student == null) return;
            await Shell.Current.GoToAsync($"StudentDetail_Route?id={student.Id}");
        }
    }
}
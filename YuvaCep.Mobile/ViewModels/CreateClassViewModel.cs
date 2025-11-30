using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace YuvaCep.Mobile.ViewModels
{

    // Listede göstereceğimiz geçici öğrenci modeli
    public class StudentItem
    {
        public string Name { get; set; }
        public string ParentName { get; set; }
        public string ParentTC { get; set; }
        public string ReferenceCode { get; set; }
    }

    public partial class CreateClassViewModel : ObservableObject
    {
        [ObservableProperty]
        private string className; //Sınıfın İsmi
        [ObservableProperty]
        private string ageGroup; //Yaş Grubu


        // --- Yeni Öğrenci Ekleme Alanı ---
        [ObservableProperty]
        private string newStudentName;
        [ObservableProperty]
        private string newParentName;
        [ObservableProperty]
        private string newParentTC;

        // Eklenen öğrencilerin listesi (Ekranda anlık görülecek.)
        public ObservableCollection<StudentItem> Students { get; } = new();

        //Rastgele referans kodu üretici fonksiyon
        private string GenerateReferenceCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [RelayCommand]
        private async Task AddStudentAsync()
        {

            //Basit Doğrulama
            if ((string.IsNullOrWhiteSpace(newStudentName)) || (string.IsNullOrWhiteSpace(newParentTC)))
            {
                await Shell.Current.DisplayAlert("Eksik Bilgi", "Lütfen öğrenci adı ve veli TC giriniz.", "Tamam");
                return;
            }

            //Öğrenciyi oluştururken Referans Kodunu da üretiyoruz
            var student = new StudentItem
            {
                Name = newStudentName,
                ParentName = newParentName,
                ParentTC = newParentTC,
                ReferenceCode = GenerateReferenceCode() //Kodu burada ürettik.
            };
            Students.Add(student);


            //Kutucukları temizler, yeni giriş yapılması için.
            newStudentName = string.Empty;
            newParentName = string.Empty;
            newParentTC = string.Empty;
        }

        [RelayCommand]
        private async Task SaveClassAsync()
        {
            if ((string.IsNullOrWhiteSpace(className)) || (Students.Count == 0))
            {
                await Shell.Current.DisplayAlert("Uyarı!", "Lütfen sınıf adı girin ve en az bir öğrenci ekleyin.", "Tamam");
                return;
            }
            //Burada sonra API'ye tüm listeyi göndereceğiz.
            await Shell.Current.DisplayAlert("Başarılı", $"{className} sınıfı ve {Students.Count} öğrenci oluşturuldu!", "Tamam");

            //Ana sayfaya yönlendiriyoruz.
            await Shell.Current.GoToAsync("TeacherHomePage");
        }

    }
}

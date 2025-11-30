using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics;

namespace YuvaCep.Mobile.ViewModels
{
    //Basit öğrenci modeli
    public class SimpleStudent
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public partial class MoodItem : ObservableObject
    {
        public string Emoji { get; set; }
        public string Description { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BorderColor))] //isSelected değişince rengi güncelle
        [NotifyPropertyChangedFor(nameof(BackgroundColor))]

        private bool isSelected;

        //Seçiliyse Mavi, değilse Şeffaf çerçeve
        public Color BorderColor => IsSelected ? Color.FromArgb("#2563EB") : Colors.Transparent;
        //Seçiliyse Açık Mavi, değilse gri arka plan
        public Color BackgroundColor => IsSelected ? Color.FromArgb("#DBEAFE") : Color.FromArgb("#F3F4F6");
    }

    public partial class DailyReportViewModel : ObservableObject
    {
        //Ekranda seçilecek öğrencilerin listesi
        public ObservableCollection<SimpleStudent> MyStudents { get; } = new();
        public ObservableCollection<MoodItem> MoodOptions { get; } = new();

        // -- FORM ALANI --
        [ObservableProperty]
        private SimpleStudent selectedStudent; //Seçilen Öğrenci

        [ObservableProperty]
        private string nutritionInfo; //Yemeğini yedi mi? (Form şeklinde olacak)

        [ObservableProperty]
        private string currentMoodText; //Keyfi nasıldı? (Emoji)

        [ObservableProperty]
        private string teacherNote; //Öğretmenin Günlük Notu

        private MoodItem _selectedMood;
        public MoodItem SelectedMood
        {
            get => _selectedMood;
            set
            {
                if (SetProperty(ref _selectedMood, value))
                {
                    if (value != null)
                    {

                        UpdateMoodSelection(value);
                    }
                }
            }
        }


        private void UpdateMoodSelection(MoodItem value)
        {
            if (value == null) return;

            //1. Tüm emojilerin seçimini kaldır (Rengi sıfırlar)
            foreach (var item in MoodOptions)
            {
                item.IsSelected = false;
            }

            //2. Yeni seçileni işaretle 
            value.IsSelected = true;

            //3. Mavi kutudaki yazıyı güncelle
            CurrentMoodText = $"{value.Emoji} {value.Description}";

        }

        public DailyReportViewModel()
        {
            //Örnek veri (Burası API ve önceki sayfadan dolacak)
            MyStudents.Add(new SimpleStudent { Name = "Ali Yılmaz" });
            MyStudents.Add(new SimpleStudent { Name = "Ayşe Demir" });
            MyStudents.Add(new SimpleStudent { Name = "Mehmet Öz" });

            MoodOptions.Add(new MoodItem { Emoji = "😭", Description = "Halsiz" });
            MoodOptions.Add(new MoodItem { Emoji = "🙁", Description = "Keyifsiz" });
            MoodOptions.Add(new MoodItem { Emoji = "😐", Description = "Normal" });
            MoodOptions.Add(new MoodItem { Emoji = "🙂", Description = "İyi" });
            MoodOptions.Add(new MoodItem { Emoji = "🤩", Description = "Harika" });

            //Varsayılan Değerler
            currentMoodText = "Seçim Yapılmadı";
            teacherNote = "Etkinliğe aktif katıldı.";

        }


        [RelayCommand]
        private async Task SaveReportAsync()
        {
            if (selectedStudent == null)
            {
                await Shell.Current.DisplayAlert("Uyarı", "Lütfen bir öğrenci seçiniz.", "Tamam");
                return;
            }
            if (currentMoodText == "Seçim Yapılmadı")
            {
                await Shell.Current.DisplayAlert("Uyarı", "Lütfen bir duygu durumu (emoji) seçiniz.", "Tamam");
                return;
            }

            //Rapor Özeti
            string message = $"{selectedStudent.Name} için rapor oluşturuldu:\n" +
                             $"Mod: {currentMoodText}\n" +
                             $"Yemek: {nutritionInfo}";

            await Shell.Current.DisplayAlert("Başarılı", message, "Tamam");

            //Kaydettikten sonra önceki sayfaya dön
            await Shell.Current.GoToAsync("..");


        }

    }

}

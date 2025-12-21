using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace YuvaCep.Mobile.ViewModels
{
    // Listede görünecek her bir satırın modeli
    public class StudentBadgeItem
    {
        public string Name { get; set; }
        public string BadgeIcon { get; set; } 
        public int CompletedDays { get; set; } 
        public bool HasBadge { get; set; } 
    }

    public partial class BadgeTrackingViewModel : ObservableObject
    {
        public ObservableCollection<StudentBadgeItem> Students { get; } = new();

        public BadgeTrackingViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            // --- TEST VERİLERİ ---

            Students.Add(new StudentBadgeItem { Name = "Ali Yılmaz", CompletedDays = 12, BadgeIcon = "🥉", HasBadge = true });
            Students.Add(new StudentBadgeItem { Name = "Zeynep Demir", CompletedDays = 28, BadgeIcon = "🥈", HasBadge = true });
            Students.Add(new StudentBadgeItem { Name = "Mert Akın", CompletedDays = 30, BadgeIcon = "🥇", HasBadge = true });

            // Rozeti olmayan öğrenci örneği (Boş Kart)
            Students.Add(new StudentBadgeItem { Name = "Ece Su", CompletedDays = 4, BadgeIcon = "🛡️", HasBadge = false });
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
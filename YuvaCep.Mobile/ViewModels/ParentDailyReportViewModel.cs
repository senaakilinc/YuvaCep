using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace YuvaCep.Mobile.ViewModels
{   
    public partial class ParentDailyReportViewModel : ObservableObject
    {
        [ObservableProperty]
        private string date;
        [ObservableProperty]
        private string moodEmoji;
        [ObservableProperty]
        private string moodTitle;
        [ObservableProperty]
        private string nutritionInfo;
        [ObservableProperty]
        private string teacherNote;
        [ObservableProperty]
        private bool hasReport;  // Rapor var mı yok mu?

        public ParentDailyReportViewModel()
        {
            LoadReport();
        }

        private void LoadReport()
        {
            // SİMÜLASYON: API'Den Bugünün raporunu çekiyoruz.
            date = DateTime.Now.ToString("dd MMMM yyyy, dddd");

            // Öğretmenin girdiği veliler (Örnek)
            moodEmoji = "🤩";
            moodTitle = "Harika";
            nutritionInfo = "Öğle yemeğinde çorbasını bitirdi, makarnadan az yedi.";
            teacherNote = "Ali bugün oyun saatinde arkadaşlarıyla çok uyumluydu.";

            hasReport = true;   

        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

    }
}

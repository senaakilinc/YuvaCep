using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;


namespace YuvaCep.Mobile.ViewModels
{
    //Öğrenci Modeli (CreateClassViewModel'dekiyle aynı yapıda)
    public class StudentViewItem
    {
        public string Name { get; set; }
        public string ParentName {  get; set; }
        public string ReferenceCode { get; set; }
        public string PhotoUrl { get; set; }

    }

    public partial class StudentListViewModel : ObservableObject
    {
        //Ekranda görünecek liste 
        public ObservableCollection<StudentViewItem> ClassRoster { get; } = new();
        public StudentListViewModel()
        {
            //Örnek veriler (DB den çekmişiz gibi)
            ClassRoster.Add(new StudentViewItem { Name = "Ali Yılmaz", ParentName = "Mehmet Yılmaz", ReferenceCode = "A7X29B" });
            ClassRoster.Add(new StudentViewItem { Name = "Ayşe Demir", ParentName = "Zeynep Demir", ReferenceCode = "K9L12M" });
            ClassRoster.Add(new StudentViewItem { Name = "Can Öz", ParentName = "Fatma Öz", ReferenceCode = "B2R44Z" });
            ClassRoster.Add(new StudentViewItem { Name = "Elif Su", ParentName = "Ahmet Su", ReferenceCode = "T8Q99P" });
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}

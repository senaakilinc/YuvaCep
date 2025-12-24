using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace YuvaCep.Mobile.ViewModels
{
    public partial class RoleSelectionViewModel : ObservableObject
    {
        // Öğretmen veya Veli butonuna basıldığında bu çalışacak
        [RelayCommand]
        private async Task SelectRoleAsync(string role)
        {
            // role parametresi "Teacher" veya "Parent" olarak gelecek.

            await Shell.Current.GoToAsync($"LoginPage?role={role}");
        }
    }
}
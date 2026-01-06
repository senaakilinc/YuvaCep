using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace YuvaCep.Mobile.ViewModels
{
    public partial class RoleSelectionViewModel : ObservableObject
    {
        [RelayCommand]
        private async Task SelectRoleAsync(string role)
        {

            await Shell.Current.GoToAsync($"LoginPage?role={role}");
        }
    }
}
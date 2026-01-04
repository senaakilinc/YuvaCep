using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views;

public partial class RoleSelectionPage : ContentPage
{
    public RoleSelectionPage(RoleSelectionViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void OnTeacherClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Login_Route?role=Teacher");
    }

    private async void OnParentClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Login_Route?role=Parent");
    }
}
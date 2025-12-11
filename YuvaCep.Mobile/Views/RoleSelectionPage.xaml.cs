namespace YuvaCep.Mobile.Views;

public partial class RoleSelectionPage : ContentPage
{
	public RoleSelectionPage()
	{
		InitializeComponent();
	}
    private async void OnParentTapped(object sender, EventArgs e)
    {
        // Login sayfasýna "Veli" olduðunu söyleyerek gidiyoruz
        await Shell.Current.GoToAsync($"LoginPage?role=Parent");
    }

    private async void OnTeacherTapped(object sender, EventArgs e)
    {
        // Login sayfasýna "Öðretmen" olduðunu söyleyerek gidiyoruz
        await Shell.Current.GoToAsync($"LoginPage?role=Teacher");
    }
}
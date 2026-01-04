namespace YuvaCep.Mobile.Views;
using YuvaCep.Mobile.ViewModels;

public partial class TeacherHomePage : ContentPage
{
	public TeacherHomePage(TeacherHomeViewModel vm)
	{
        InitializeComponent();
		BindingContext = vm;
	}
}
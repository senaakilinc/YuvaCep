using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm; 
    }
}
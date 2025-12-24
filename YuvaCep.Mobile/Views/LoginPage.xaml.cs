using YuvaCep.Mobile.Services;
using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            var authService = new AuthService();
            var viewModel = new LoginViewModel(authService);

            BindingContext = viewModel;
        }
    }
}
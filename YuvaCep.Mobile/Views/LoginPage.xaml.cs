using YuvaCep.Mobile.Services;
using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views
{

    [QueryProperty(nameof(UserRoleParameter), "role")]
    public partial class LoginPage : ContentPage
    {
        // Parametre buraya düþer
        private string _userRoleParameter;
        public string UserRoleParameter
        {
            get => _userRoleParameter;
            set
            {
                _userRoleParameter = value;

                if (BindingContext is LoginViewModel vm)
                {
                    vm.UserRole = value;
                }
            }
        }

        public LoginPage()
        {
            InitializeComponent();
            var authService = new AuthService();
            BindingContext = new LoginViewModel(authService);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ApplyTheme(_userRoleParameter);
        }

        private void ApplyTheme(string role)
        {

            if (string.IsNullOrEmpty(role)) return;

            bool isTeacher = role == "Teacher";

            string colorKey = isTeacher ? "PrimaryColor" : "SecondaryColor";

            if (Application.Current.Resources.TryGetValue(colorKey, out var colorObj))
            {
                var themeColor = (Color)colorObj;

                if (HeaderBackground != null) HeaderBackground.Color = themeColor;
                if (LoginBtn != null) LoginBtn.BackgroundColor = themeColor;
                if (RememberMeCheck != null) RememberMeCheck.Color = themeColor;
                if (RegisterLabel != null) RegisterLabel.TextColor = themeColor;
            }

            if (RoleImage != null)
            {
                if (isTeacher)
                {
                    RoleImage.Source = "icon_teacher.png";
                }
                else
                {
                    RoleImage.Source = "icon_parent.png";
                }
            }
        }
    }
}
using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views;

[QueryProperty(nameof(RoleParameter), "role")]
public partial class RegisterPage : ContentPage
{
    private string _roleParameter;
    public string RoleParameter
    {
        get => _roleParameter;
        set
        {
            _roleParameter = value;
            ApplyTheme(value);
        }
    }

    public RegisterPage(RegisterViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void ApplyTheme(string role)
    {
        if (string.IsNullOrEmpty(role)) return;

        bool isTeacher = role == "Teacher";
        string colorKey = isTeacher ? "PrimaryColor" : "ParentColor";

        if (!Application.Current.Resources.TryGetValue(colorKey, out var colorObj))
        {
            colorKey = "SecondaryColor";
            Application.Current.Resources.TryGetValue(colorKey, out colorObj);
        }

        if (colorObj is Color themeColor)
        {
            if (HeaderBackground != null) HeaderBackground.Color = themeColor;

            if (RegisterBtn != null) RegisterBtn.BackgroundColor = themeColor;

            if (TitleLabel != null) TitleLabel.TextColor = themeColor;
        }

        if (RoleImage != null)
        {
            RoleImage.Source = isTeacher ? "icon_teacher.png" : "icon_parent.png";
        }
    }
}
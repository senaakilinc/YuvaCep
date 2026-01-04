using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views;

public partial class CreateClassPage : ContentPage
{
    public CreateClassPage(CreateClassViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
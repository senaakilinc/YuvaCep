using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views;

public partial class CreateActivityPage : ContentPage
{
    public CreateActivityPage(CreateActivityViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
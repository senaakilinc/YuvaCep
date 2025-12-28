using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views;

public partial class ParentHomePage : ContentPage
{
    public ParentHomePage(ParentHomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
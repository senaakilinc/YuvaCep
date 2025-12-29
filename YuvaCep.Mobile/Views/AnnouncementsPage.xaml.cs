using YuvaCep.Mobile.ViewModels;
namespace YuvaCep.Mobile.Views;

public partial class AnnouncementsPage : ContentPage
{
    public AnnouncementsPage(AnnouncementsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
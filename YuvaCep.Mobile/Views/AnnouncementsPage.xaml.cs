using YuvaCep.Mobile.ViewModels;
namespace YuvaCep.Mobile.Views;

public partial class AnnouncementsPage : ContentPage
{
    public AnnouncementsPage(AnnouncementsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AnnouncementsViewModel vm)
        {
            vm.LoadAnnouncementsCommand.Execute(null);
        }
    }
}
using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views;

public partial class ActivityChartDetailPage : ContentPage
{
    public ActivityChartDetailPage(ActivityChartDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
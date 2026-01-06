using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views;

public partial class StudentChartDetailPage : ContentPage
{
    public StudentChartDetailPage(StudentChartDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
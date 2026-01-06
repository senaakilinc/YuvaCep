using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views;

public partial class StudentChartsListPage : ContentPage
{
    public StudentChartsListPage(StudentChartsListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
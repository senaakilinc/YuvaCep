using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views;

public partial class StudentDetailPage : ContentPage
{
    public StudentDetailPage(StudentDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
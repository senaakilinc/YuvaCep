using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views;

public partial class StudentCardsPage : ContentPage
{
    public StudentCardsPage(StudentCardsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
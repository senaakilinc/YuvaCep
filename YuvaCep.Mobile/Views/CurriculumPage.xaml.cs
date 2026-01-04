using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views;

public partial class CurriculumPage : ContentPage
{
    public CurriculumPage(CurriculumViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is CurriculumViewModel vm)
        {
            vm.LoadDataCommand.Execute(null);
        }
    }
}
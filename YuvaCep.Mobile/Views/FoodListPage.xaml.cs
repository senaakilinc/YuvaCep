using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views;

public partial class FoodListPage : ContentPage
{
    public FoodListPage(FoodListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is FoodListViewModel vm)
        {
            vm.LoadDataCommand.Execute(null);
        }
    }
}
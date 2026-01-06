using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views
{
    public partial class ActivityChartsListPage : ContentPage
    {
        public ActivityChartsListPage(ActivityChartsListViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ActivityChartsListViewModel vm)
            {
                await vm.LoadChartsAsync();
            }
        }
    }
}
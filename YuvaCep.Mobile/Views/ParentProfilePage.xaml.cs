using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views
{
    public partial class ParentProfilePage : ContentPage
    {
        private readonly ParentProfileViewModel _viewModel;

        public ParentProfilePage(ParentProfileViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_viewModel != null)
            {
                await _viewModel.LoadUserDataAsync();
            }
        }
    }
}
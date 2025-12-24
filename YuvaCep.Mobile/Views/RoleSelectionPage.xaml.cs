using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views
{
    public partial class RoleSelectionPage : ContentPage
    {
        public RoleSelectionPage()
        {
            InitializeComponent();
            BindingContext = new RoleSelectionViewModel();
        }
    }
}
using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views
{
    public partial class StudentChartDetailPage : ContentPage
    {
        public StudentChartDetailPage(StudentChartDetailViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        private async void OnDrop(object sender, DropEventArgs e)
        {

            if (sender is VisualElement target)
            {
                await target.ScaleTo(1.1, 100);
                await target.ScaleTo(1.0, 100);
            }

            if (BindingContext is StudentChartDetailViewModel vm)
            {
                await vm.CompleteActivityAsync();
            }
        }
    }
}
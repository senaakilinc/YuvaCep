namespace YuvaCep.Mobile.Views;

public partial class DailyReportPage : ContentPage
{
    public DailyReportPage(ViewModels.DailyReportViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (Shell.Current.Navigation.NavigationStack.Count > 1)
            {
                await Shell.Current.Navigation.PopAsync();
            }
            else
            {
                await Shell.Current.GoToAsync("..");
            }
        }
        catch
        {
            await Shell.Current.GoToAsync("//ParentHomePage");
        }
    }
}
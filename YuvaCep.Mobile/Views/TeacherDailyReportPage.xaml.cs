using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views;

public partial class TeacherDailyReportPage : ContentPage
{
    public TeacherDailyReportPage(TeacherDailyReportViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
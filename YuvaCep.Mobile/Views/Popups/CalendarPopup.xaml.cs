using CommunityToolkit.Maui.Views;
using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views.Popups
{
    public partial class CalendarPopup : Popup
    {
        public CalendarPopup(List<StudentDayModel> days, string studentName)
        {
            InitializeComponent();

            PopupTitle.Text = studentName;
            CalendarCollection.ItemsSource = days;
        }

        private void OnCloseClicked(object sender, EventArgs e)
        {
            CloseAsync();
        }
    }
}
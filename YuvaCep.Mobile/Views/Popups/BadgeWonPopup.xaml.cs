using CommunityToolkit.Maui.Views;
using YuvaCep.Mobile.Dtos;

namespace YuvaCep.Mobile.Views.Popups
{
    public partial class BadgeWonPopup : Popup
    {
        public BadgeWonPopup(NewBadgeDto badge)
        {
            InitializeComponent();

            BadgeNameLabel.Text = badge.Name;
            BadgeDescLabel.Text = badge.Description;
            BadgeImage.Source = badge.ImageUrl; 
        }

        private void OnCloseClicked(object sender, EventArgs e)
        {
            CloseAsync();
        }
    }
}
using JimmyToolbox.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.Storage;

namespace JimmyToolbox.Views
{
    public sealed partial class WOLPage : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public WOLPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (localSettings.Values.ContainsKey("wolmacaddress"))
                    DeviceMacTextBox.Text = localSettings.Values["wolmacaddress"].ToString();
            }
            catch
            {

            }
        }

        private async void WOLButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(DeviceMacTextBox.Text))
            {
                bool isAwake = await WakeOnLanService.SendWakeOnLan(DeviceMacTextBox.Text);
                if (isAwake)
                {
                    localSettings.Values["wolmacaddress"] = DeviceMacTextBox.Text;
                    WakeTextBlock.Text = "Sent";
                }
                else
                {
                    WakeTextBlock.Text = "Failed";
                }
            }
        }
    }
}

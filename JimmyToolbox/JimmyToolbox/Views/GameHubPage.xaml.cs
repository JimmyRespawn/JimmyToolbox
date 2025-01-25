using JimmyToolbox.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using JimmyToolbox.Services;

namespace JimmyToolbox.Views
{
    public sealed partial class GameHubPage : Page
    {
        public GameHubPage()
        {
            this.InitializeComponent();
        }

        private void LaunchGame(string uri)
        {
            GameLauncher.LaunchSteamGame(uri);
        }

        private void LaunchGameButton_Click(object sender, RoutedEventArgs e)
        {
            GameLauncher.LaunchSteamGame("1677280");
        }

        private void LaunchCS2Button_Click(object sender, RoutedEventArgs e)
        {
            GameLauncher.LaunchSteamGame("730");
        }

        private void LaunchUWPGameButton_Click(object sender, RoutedEventArgs e)
        {
            GameLauncher.LaunchUWPGame("774f87f6");
        }

        private void LaunchDiscordButton_Click(object sender, RoutedEventArgs e)
        {
            GameLauncher.LunachGame("DISCORD://");
        }

        private void LaunchUUButton_Click(object sender, RoutedEventArgs e)
        {
            GameLauncher.LunachGame("uu://");
        }

        private void LaunchUWPSingleGameButton_Click(object sender, RoutedEventArgs e)
        {
            GameLauncher.LaunchUWPGame("7782504a");
        }
    }
}

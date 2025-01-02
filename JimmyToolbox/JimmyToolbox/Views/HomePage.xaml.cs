using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using Windows.System;

namespace JimmyToolbox.Views
{
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
#if DEBUG
            GameLauncherButton.IsHitTestVisible = true;
            WebpageButton.IsHitTestVisible = true;
#endif
        }

        private void LetterCounterButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LetterCountPage));
        }

        private void WOLButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WOLPage));
        }

        private void CurrentcyConverterButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CurrencyPage));
        }

        private async void IdeaButton_Click(object sender, RoutedEventArgs e)
        {
            var feedbackUri = new Uri("ms-windows-store://review/?ProductId=9WZDNCRDXTF1");

            bool success = await Launcher.LaunchUriAsync(feedbackUri);

            if (!success)
            {
                System.Diagnostics.Debug.WriteLine("Failed to launch Feedback Hub.");
            }
        }

        private void ChineseConverterButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ChineseConvertPage));
        }

        private void GameLauncherButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GameHubPage));
        }

        private void WebpageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WebPage));
        }
    }
}

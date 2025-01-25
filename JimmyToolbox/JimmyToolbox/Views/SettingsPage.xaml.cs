using System;
using System.Threading.Tasks;
using Windows.Globalization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using JimmyToolBox.Helpers;
using Windows.Storage;
using Microsoft.UI.Xaml.Navigation;

namespace JimmyToolbox.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LoadSettings();
        }

        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private void LoadSettings()
        {

            if (localSettings.Values["theme"] != null)
            {
                string theme = localSettings.Values["theme"].ToString();
                if (theme == "Light")
                    ThemeComboBox.SelectedIndex = 1;
                if (theme == "Dark")
                    ThemeComboBox.SelectedIndex = 2;
                if (theme == "Auto")
                    ThemeComboBox.SelectedIndex = 0;
            }
            else
            {
                ThemeComboBox.SelectedIndex = 0;
            }


            string appLanguage = ApplicationLanguages.PrimaryLanguageOverride;
            if (!string.IsNullOrEmpty(appLanguage))
            {
                switch (appLanguage)
                {
                    case "en-US":
                        LanguageCombobox.SelectedIndex = 1;
                        break;
                    case "zh-Hans":
                        LanguageCombobox.SelectedIndex = 2;
                        break;
                }
            }
            else
                LanguageCombobox.SelectedIndex = 0;


            //Refresh Version
            var ver = Windows.ApplicationModel.Package.Current.Id.Version;
            Version.Text = ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Build.ToString();
#if DEBUG
            Version.Text += " (Debug)";
#endif
        }

        private async void RateButton_Click(object sender, RoutedEventArgs e)
        {
            await GeneralHelper.Rate("9WZDNCRDXTF1");
        }

        private void ChangeLanguageButton_Click(object sender, RoutedEventArgs e)
        {
            this.LanguageCombobox.IsDropDownOpen = !this.LanguageCombobox.IsDropDownOpen;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string appLanguage = ApplicationLanguages.PrimaryLanguageOverride;
            int languangeIndex = LanguageCombobox.SelectedIndex;
            switch (languangeIndex)
            {
                case 1:
                    if (appLanguage != "en-US")
                        ChangeAppLanguage("en-US");
                    break;
                case 2:
                    if (appLanguage != "zh-Hans")
                        ChangeAppLanguage("zh-hans");
                    break;
                case 0:
                    if (appLanguage != "")
                        ChangeAppLanguage("Default");
                    break;
            }
        }
        private async void ChangeAppLanguage(string languageCode)
        {
            if (languageCode == "Default")
            {
                Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = string.Empty;
                await Task.Delay(100);
                Frame.Navigate(this.GetType());
                await Windows.ApplicationModel.Core.CoreApplication.RequestRestartAsync(string.Empty);
                return;
            }
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = languageCode;
            await Task.Delay(100);
            Frame.Navigate(this.GetType());
            await Windows.ApplicationModel.Core.CoreApplication.RequestRestartAsync(string.Empty);
        }

        private void ThmeButton_Click(object sender, RoutedEventArgs e)
        {
            this.ThemeComboBox.IsDropDownOpen = !this.ThemeComboBox.IsDropDownOpen;
        }

        private void LightThemeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeComboBox.SelectedIndex == 0)
                GeneralHelper.ThemeSwitchAsync("Auto");
            else if (ThemeComboBox.SelectedIndex == 1)
            {
                GeneralHelper.ThemeSwitchAsync("Light");
                ThemeText.Text = "\uE706";
            }
            else if (ThemeComboBox.SelectedIndex == 2)
            {
                GeneralHelper.ThemeSwitchAsync("Dark");
                ThemeText.Text = "\uE708";
            }
        }
    }
}

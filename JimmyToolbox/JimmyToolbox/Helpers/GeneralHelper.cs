using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Microsoft.UI;
using JimmyToolbox;

namespace JimmyToolBox.Helpers
{
    public class GeneralHelper
    {
        public static Windows.UI.Color ConvertStringToColor(String hex)
        {
            //remove the # at the front
            hex = hex.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            int start = 0;

            //handle ARGB strings (8 characters long)
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }

            //convert RGB characters to bytes
            r = byte.Parse(hex.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return Windows.UI.Color.FromArgb(a, r, g, b);
        }

        public static async Task<bool> Rate(string StoreProduectID)
        {
            try
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?ProductId=" + StoreProduectID));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async static Task<bool> ThemeSwitchAsync(string theme)
        {
            try
            {
                Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["theme"] = theme;
                if (theme == "Auto")
                {
                    UISettings uiSettings = new UISettings();
                    var backgroundColor = uiSettings.GetColorValue(UIColorType.Background);
                    theme = "Light";
                    if (backgroundColor == Colors.Black)
                        theme = "Dark";
                }

                var app = (App)Application.Current;
                if (theme == "Dark")
                {
                    if (app.m_window.Content is FrameworkElement rootElement)
                        rootElement.RequestedTheme = ElementTheme.Dark; // 或 ElementTheme.Light
                    //ttb.ForegroundColor = Colors.White;
                    //ttb.ButtonForegroundColor = Colors.White;
                }
                else if (theme == "Light")
                {
                    if (app.m_window.Content is FrameworkElement rootElement)
                        rootElement.RequestedTheme = ElementTheme.Light; // 或 ElementTheme.Light
                    //ttb.ForegroundColor = Windows.UI.Colors.Black;
                    //ttb.ButtonForegroundColor = Windows.UI.Colors.Black;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
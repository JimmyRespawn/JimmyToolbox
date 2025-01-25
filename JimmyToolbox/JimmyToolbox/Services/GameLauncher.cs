using System;
using Windows.System;

namespace JimmyToolbox.Services
{
    public class GameLauncher
    {
        public static async void LaunchSteamGame(string steamid)
        {
            string steamUri = "steam://rungameid/" + steamid;
            LunachGame(steamUri);
        }

        public static async void LaunchUWPGame(string uwpid)
        {
            //Process.Start("explorer.exe", "shell:AppsFolder\\" + uwpid);
            string xboxUri = "ms-xbl-" + uwpid + ":";
            LunachGame(xboxUri);
        }

        public static async void LunachGame(string url)
        {
            var uri = new Uri(url);
            bool success = await Launcher.LaunchUriAsync(uri);
            if (!success)
                await new Windows.UI.Popups.MessageDialog("Unable to launch the game, please ensure you have installed the game").ShowAsync();
            // 如果失败，可以提示用户或采取其他操作
        }
    }
}

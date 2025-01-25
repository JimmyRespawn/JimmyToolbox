using Microsoft.UI.Xaml;
using Windows.Storage;
using JimmyToolBox.Helpers;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using Microsoft.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace JimmyToolbox
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public Window? m_window;

        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
            DetermineAppTheme();
            //m_window.Closed += OnMainWindowClosed; // 窗口关闭时保存数据
            //RestoreWindowPositionAndSize(m_window); // 恢复窗口位置和大小
        }

        private void DetermineAppTheme()
        {
            try
            {
                // Get the requested theme from the ApplicationData
                var theme = ApplicationData.Current.LocalSettings.Values["theme"];
                if (theme != null)
                {
                    // Set the RequestedTheme to the saved theme
                    string RequestedTheme = theme.ToString();
                    GeneralHelper.ThemeSwitchAsync(RequestedTheme);
                }
            }
            catch
            {

            }
        }

        private void OnMainWindowClosed(object sender, WindowEventArgs args)
        {
            SaveWindowPositionAndSize(m_window); // 保存窗口位置和大小
        }

        private void SaveWindowPositionAndSize(Window window)
        {
            if (window is not null)
            {
                var settings = ApplicationData.Current.LocalSettings;
                settings.Values["WindowWidth"] = window.Bounds.Width;
                settings.Values["WindowHeight"] = window.Bounds.Height;
                var appWindow = window.AppWindow;
                if (appWindow is not null)
                {
                    settings.Values["WindowX"] = appWindow.Position.X;
                    settings.Values["WindowY"] = appWindow.Position.Y;

                    // 判断并保存是否最大化或全屏
                }
            }
        }

        private void RestoreWindowPositionAndSize(Window window)
        {
            var settings = ApplicationData.Current.LocalSettings;

            var appWindow = window.AppWindow;
            if (settings.Values.ContainsKey("WindowWidth") &&
                settings.Values.ContainsKey("WindowHeight") &&
                settings.Values.ContainsKey("WindowX") &&
                settings.Values.ContainsKey("WindowY"))
            {
                var width = (double)settings.Values["WindowWidth"];
                var height = (double)settings.Values["WindowHeight"];
                var x = (int)settings.Values["WindowX"];
                var y = (int)settings.Values["WindowY"];
                appWindow.Resize(new Windows.Graphics.SizeInt32((int)width, (int)height));
                appWindow.Move(new Windows.Graphics.PointInt32(x, y));
            }
        }
    }
}

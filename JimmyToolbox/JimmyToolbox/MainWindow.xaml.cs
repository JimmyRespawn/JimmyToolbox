using System;
using System.Runtime.InteropServices;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Composition.SystemBackdrops;
using WinRT.Interop;
using JimmyToolbox.Views;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Core;
using Microsoft.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;

namespace JimmyToolbox
{
    public sealed partial class MainWindow : Window
    {
        private double MinWidth = 480;
        private double MinHeight = 500;

        public MainWindow()
        {
            this.InitializeComponent();
            try
            {
                //Set Header Title
                this.ExtendsContentIntoTitleBar = true;
                this.SetTitleBar(AppTitleBar);

                //Set app icon
                SetWindowIcon("Assets/icon.ico");

                //Set Background
                ApplyBackdrop();
                //Navigation

                //Get the Window Scale
                IntPtr hwnd = WindowNative.GetWindowHandle(this);
                double dpi = (GetDpiForWindow(hwnd) / 96d);
                MinWidth = MinWidth * dpi;
                MinHeight = MinHeight * dpi;

                //Set Window Min Size
                hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
                SubClassDelegate = new SUBCLASSPROC(WindowSubClass);
                bool bReturn = SetWindowSubclass(hWnd, SubClassDelegate, 0, 0);

#if DEBUG
                GameHubNVI.Visibility = Visibility.Visible;
                WebPageNVI.Visibility = Visibility.Visible;
#endif
            }
            catch
            {

            }
        }

        private void NavigationViewControl_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected == true)
            {
                NavView_Navigate("settings");
            }
            //else if (NavigationViewControl.SelectedItem == RecordButton)
            //{
            //    if (!isRecording)
            //    {
            //        if (_device == null)
            //            _device = Direct3D11Helpers.CreateDevice();
            //        StarRecordAsync();
            //        await Task.Delay(150);
            //        NavigationViewControl.SelectedItem = HomePageNVI;
            //    }
            //    else
            //    {
            //        _encoder?.Dispose();
            //        RecordFontIcon.Glyph = "\uEA3F";
            //        var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            //        RecordButton.Content = resourceLoader.GetString("Record/Content");
            //        isRecording = false;
            //        await Task.Delay(150);
            //        NavigationViewControl.SelectedItem = HomePageNVI;
            //    }
            //    return;
            //}
            else if (args.SelectedItemContainer != null)
            {
                var navItemTag = args.SelectedItemContainer.Tag.ToString();
                //NavigationViewControl.PaneDisplayMode = muxc.NavigationViewPaneDisplayMode.Auto;

                NavView_Navigate(navItemTag);
            }
            NavigationViewControl.Header = "";
        }

        private void NavView_Navigate(string navItemTag)
        {
            Type _page = null;

            if (navItemTag == "settings")
            {
                //_page = typeof(Views.SettingsPage);
            }
            else
            {
                var item = _pages.FirstOrDefault(p => p.Tag.Equals(navItemTag));
                _page = item.Page;
            }
            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            var preNavPageType = ContentFrame.CurrentSourcePageType;

            //if (navItemTag == "downloads" || navItemTag == "collected" || navItemTag == "unplayed")
            //{
            //    ContentFrame.Navigate(_page, navItemTag, transitionInfo);
            //    return;
            //}

            // Only navigate if the selected page isn't currently loaded.
            if (!(_page is null) && !Type.Equals(preNavPageType, _page))
            {
                ContentFrame.Navigate(_page, null);
            }
        }

        private void ApplyBackdrop()
        {
            SystemBackdrop = new MicaBackdrop()
            { Kind = MicaKind.BaseAlt };
        }

        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            ("lettercounter", typeof(Views.LetterCountPage)),
            ("wol", typeof(Views.WOLPage)),
            ("currency", typeof(Views.CurrencyPage)),
            ("chinese", typeof(Views.ChineseConvertPage)),
            ("gamelauncher", typeof(Views.GameHubPage)),
            ("webbookmarks", typeof(Views.WebPage)),
            ("home", typeof(Views.HomePage))
        };

        // P/Invoke µ÷ÓÃ GetDpiForWindow º¯Êý
        [DllImport("user32.dll")]
        public static extern int GetDpiForWindow(IntPtr hWnd);

        //Set Minimum Size
        IntPtr hWnd = IntPtr.Zero;
        private SUBCLASSPROC SubClassDelegate;
        private int WindowSubClass(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, uint dwRefData)
        {
            switch (uMsg)
            {
                case WM_GETMINMAXINFO:
                    {
                        MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
                        mmi.ptMinTrackSize.X = (int)MinWidth;
                        mmi.ptMinTrackSize.Y = (int)MinHeight;
                        Marshal.StructureToPtr(mmi, lParam, false);
                        return 0;
                    }
                    break;
            }
            return DefSubclassProc(hWnd, uMsg, wParam, lParam);
        }


        public delegate int SUBCLASSPROC(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, uint dwRefData);

        [DllImport("Comctl32.dll", SetLastError = true)]
        public static extern bool SetWindowSubclass(IntPtr hWnd, SUBCLASSPROC pfnSubclass, uint uIdSubclass, uint dwRefData);

        [DllImport("Comctl32.dll", SetLastError = true)]
        public static extern int DefSubclassProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        public const int WM_GETMINMAXINFO = 0x0024;

        public struct MINMAXINFO
        {
            public System.Drawing.Point ptReserved;
            public System.Drawing.Point ptMaxSize;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Point ptMinTrackSize;
            public System.Drawing.Point ptMaxTrackSize;
        }

        // Set Window Icon
        private void SetWindowIcon(string iconPath)
        {
            var hwnd = WindowNative.GetWindowHandle(this);
            IntPtr hIcon = LoadImage(IntPtr.Zero, iconPath, IMAGE_ICON, 0, 0, LR_LOADFROMFILE | LR_DEFAULTSIZE);
            SendMessage(hwnd, WM_SETICON, ICON_BIG, hIcon);
            SendMessage(hwnd, WM_SETICON, ICON_SMALL, hIcon);
        }
        private const int WM_SETICON = 0x80;
        private const int ICON_SMALL = 0;
        private const int ICON_BIG = 1;
        private const int IMAGE_ICON = 1;
        private const int LR_LOADFROMFILE = 0x10;
        private const int LR_DEFAULTSIZE = 0x40;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, IntPtr lParam);

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            // Add handler for ContentFrame navigation.
            ContentFrame.Navigated += On_Navigated;

            // NavView doesn't load any page by default, so load home page.
            NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems[0];
            // If navigation occurs on SelectionChanged, this isn't needed.
            // Because we use ItemInvoked to navigate, we need to call Navigate
            // here to load the home page.
            //NavView_Navigate("video", new Windows.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo());

            // Listen to the window directly so the app responds
            // to accelerator keys regardless of which element has focus.
            //Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated +=
            //    CoreDispatcher_AcceleratorKeyActivated;

            //Window.Current.CoreWindow.PointerPressed += CoreWindow_PointerPressed;

        }

        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            NavigationViewControl.IsBackEnabled = ContentFrame.CanGoBack;

            if (ContentFrame.SourcePageType == typeof(Views.SettingsPage))
            {
                // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag.
                NavigationViewControl.SelectedItem = (NavigationViewItem)NavigationViewControl.SettingsItem;
                NavigationViewControl.Header = "Settings";
            }
            else if (ContentFrame.SourcePageType != null)
            {
                var item = _pages.FirstOrDefault(p => p.Page == e.SourcePageType);

                NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems
                    .OfType<NavigationViewItem>()
                    .First(n => n.Tag.Equals(item.Tag));

                //NavigationViewControl.Header =
                //    ((muxc.NavigationViewItem)NavigationViewControl.SelectedItem)?.Content?.ToString();
            }
            NavigationViewControl.Header = "";
        }

        //private void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs e)
        //{
        //    // Handle mouse back button.
        //    if (e.CurrentPoint.Properties.IsXButton1Pressed)
        //    {
        //        e.Handled = TryGoBack();
        //    }
        //}

        //private void CoreDispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs e)
        //{
        //    // When Alt+Left are pressed navigate back
        //    if (e.EventType == CoreAcceleratorKeyEventType.SystemKeyDown
        //        && e.VirtualKey == Windows.System.VirtualKey.Left
        //        && e.KeyStatus.IsMenuKeyDown == true
        //        && !e.Handled)
        //    {
        //        e.Handled = TryGoBack();
        //    }
        //}

        private void System_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = TryGoBack();
            }
        }
        

        private bool TryGoBack()
        {
            if (!ContentFrame.CanGoBack)
                return false;

            // Don't go back if the nav pane is overlayed.
            if (NavigationViewControl.IsPaneOpen &&
                (NavigationViewControl.DisplayMode == NavigationViewDisplayMode.Compact ||
                 NavigationViewControl.DisplayMode == NavigationViewDisplayMode.Minimal))
                return false;

            ContentFrame.GoBack();
            return true;
        }

        private void NavigationViewControl_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            TryGoBack();
        }

        private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
        {
            const int topIndent = 16;
            const int expandedIndent = 48;
            int minimalIndent = 104;

            // If the back button is not visible, reduce the TitleBar content indent.
            if (NavigationViewControl.IsBackButtonVisible.Equals(Microsoft.UI.Xaml.Controls.NavigationViewBackButtonVisible.Collapsed))
                minimalIndent = 48;

            Thickness currMargin = AppTitleBar.Margin;

            // Set the TitleBar margin dependent on NavigationView display mode
            if (sender.PaneDisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.Top)
            {
                AppTitle.Visibility = Visibility.Collapsed;
                AppFontIcon.Visibility = Visibility.Collapsed;
                AppTitleBar.Margin = new Thickness(topIndent, currMargin.Top, currMargin.Right, currMargin.Bottom);
                ContentFrame.Margin = new Thickness(0, 0, 0, 0);
                MainContentFrame.Margin = new Thickness(0, 0, 0, 0);
            }
            else if (sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Minimal)
            {
                AppTitle.Visibility = Visibility.Collapsed;
                AppFontIcon.Visibility = Visibility.Collapsed;
                AppTitleBar.Margin = new Thickness(minimalIndent, currMargin.Top, currMargin.Right, currMargin.Bottom);
                MainContentFrame.Margin = new Thickness(0, -80, 0, 0);
            }
            else
            {
                //AppTitle.Visibility = Visibility.Visible;
                //AppFontIcon.Visibility = Visibility.Visible;
                AppTitleBar.Margin = new Thickness(expandedIndent, currMargin.Top, currMargin.Right, currMargin.Bottom);
                MainContentFrame.Margin = new Thickness(0, 0, 0, 0);
                ContentFrame.Margin = new Thickness(0, 0, 0, 0);
            }
        }
    }
}
using JimmyToolbox.Services;
using System.Threading;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using System;

namespace JimmyToolbox.Views
{
    public sealed partial class ShutDownPage : Page
    {
        public ShutDownPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Microsoft.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TimeTypeCB.SelectedIndex == 0)
                PowerServices.RestartComputer();
            else if (TimeTypeCB.SelectedIndex == 1)
                ScheduleOperation(InTimePicker.Time, 1);
            else if (TimeTypeCB.SelectedIndex == 2)
                ScheduleOperation(TimefromNow(SpecificTimePicker.Time), 1);
        }

        private void ShutDownButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TimeTypeCB.SelectedIndex == 0)
                    PowerServices.ShutdownComputer();
                else if (TimeTypeCB.SelectedIndex == 1)
                    ScheduleOperation(InTimePicker.Time);
                else if (TimeTypeCB.SelectedIndex == 2)
                    ScheduleOperation(TimefromNow(SpecificTimePicker.Time));
            }
            catch
            {

            }
        }

        private TimeSpan TimefromNow(TimeSpan time)
        {
            return time - DateTime.Now.TimeOfDay;
        }

        public void ScheduleOperation(TimeSpan delay, int operationType = 0)
        {
            if (MainWindow._cancellationTokenSource == null)
                MainWindow._cancellationTokenSource = new CancellationTokenSource();
            else
                MainWindow._cancellationTokenSource.Cancel();
            var cancellationToken = MainWindow._cancellationTokenSource.Token;

            //Console.WriteLine($"Scheduled shutdown in {delay.TotalSeconds} seconds");
            string time = DateTime.Now.Add(delay).ToString("HH:mm:ss");
            OutputBox.Text = $"Scheduled at {time}";
            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(delay, cancellationToken); // 等待指定时间或取消
                    if(operationType == 0)
                        await PowerServices.ShutdownComputer();
                    else
                        await PowerServices.RestartComputer();
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Operation has been cancelled.");
                }
            }, cancellationToken);
        }

        public void CancelOperation()
        {
            if (MainWindow._cancellationTokenSource != null)
            {
                MainWindow._cancellationTokenSource.Cancel();
                MainWindow._cancellationTokenSource.Dispose();
                MainWindow._cancellationTokenSource = null;
            }
        }

        private void TimeTypeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? cb = sender as ComboBox;
            var cbi = cb.SelectedItem as ComboBoxItem;
            if (cbi == null)
                return;   
            if (cbi.Tag.ToString() == "in")
            {
                InTimePicker.Visibility = Visibility.Visible;
                SpecificTimePicker.Visibility = Visibility.Collapsed;
            }
            else if(cbi.Tag.ToString() == "at")
            {
                InTimePicker.Visibility = Visibility.Collapsed;
                SpecificTimePicker.Visibility = Visibility.Visible;
            }
            else
            {
                InTimePicker.Visibility = Visibility.Collapsed;
                SpecificTimePicker.Visibility = Visibility.Collapsed;
            }
        }

        private void TimeTypeCB_Loaded(object sender, RoutedEventArgs e)
        {
            TimeTypeCB.SelectionChanged += TimeTypeCB_SelectionChanged;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow._cancellationTokenSource != null)
            {
                CancelOperation();
                OutputBox.Text = "Operation cancelled.";
            }
        }
    }
}
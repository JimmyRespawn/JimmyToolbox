using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace JimmyToolbox.Views
{
    public sealed partial class ScriptsPage : Page
    {
        public ScriptsPage()
        {
            this.InitializeComponent();
        }

        private async void RunScriptsButton_Click(object sender, RoutedEventArgs e)
        {
            string command = ScriptsLocation.Text;
            var output = await RunCommandAsync("powershell.exe", $"-Command \"{command}\"");
            ScriptsLocation.Text = output;
        }

        private Task<string> RunCommandAsync(string fileName, string arguments)
        {
            return Task.Run(() =>
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };

                using (Process process = new Process { StartInfo = startInfo })
                {
                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(error))
                    {
                        output += $"\n¥ÌŒÛ–≈œ¢:\n{error}";
                    }

                    return output;
                }
            });
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string command = @"C:\Users\jimmy\Desktop\DeployWebsites.bat";
            var output = await RunCommandAsync("powershell.exe", $"-Command \"{command}\"");
            ScriptsLocation.Text = output;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string command = @"C:\Users\jimmy\Desktop\SyncWebsites.bat";
            var output = await RunCommandAsync("powershell.exe", $"-Command \"{command}\"");
            ScriptsLocation.Text = output;
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
        }
    }
}

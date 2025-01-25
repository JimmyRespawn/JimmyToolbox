using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Threading.Tasks;
using OpenCCNET;

namespace JimmyToolbox.Views
{
    public sealed partial class ChineseConvertPage : Page
    {
        public ChineseConvertPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //try
            //{
            //    await Task.Delay(250);
            //    //ZhConverter.Initialize();
            //}
            //catch
            //{

            //}
        }

        private async void TransformChineseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SourceChineseTextBox.Text))
                TargetChineseTextBox.Text = await Simplifield2TraditionalChineseConvertAsync(SourceChineseTextBox.Text, true);
        }

        private async void TransformChineseButton2_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SourceChineseTextBox.Text))
                TargetChineseTextBox.Text = await Simplifield2TraditionalChineseConvertAsync(SourceChineseTextBox.Text, false);
        }

        private async Task<string> Simplifield2TraditionalChineseConvertAsync(string sourceString, bool convertType)
        {
            // convertType£º true = simp to trad, false = trad to simp
            if (!string.IsNullOrEmpty(sourceString))
            {
                string ret = "";
                if (convertType)
                    ret = ZhConverter.HansToHant(sourceString);//Ê¹ÓÃÊ²üNÞD“QÒŽ„t
                else
                    ret = ZhConverter.HantToHans(sourceString);
                return ret;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}

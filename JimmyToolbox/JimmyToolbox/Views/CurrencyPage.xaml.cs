using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Diagnostics;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace JimmyToolbox.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CurrencyPage : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public CurrencyPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (localSettings.Values.ContainsKey("currencyFrom"))
                SelectComboBoxItemByContent(SourceCurrencyComboBox, localSettings.Values["currencyFrom"].ToString());
            else
                SourceCurrencyComboBox.SelectedIndex = 0;

            if (localSettings.Values.ContainsKey("currencyTo"))
                SelectComboBoxItemByContent(TargetCurrencyComboBox, localSettings.Values["currencyTo"].ToString());
            else
                TargetCurrencyComboBox.SelectedIndex = 0;
        }

        private async void TransformMoneyButton_Click(object sender, RoutedEventArgs e)
        {
            ProgressBar1.IsIndeterminate = true;
            string sourceCurrencyText = "100";
            if (!string.IsNullOrEmpty(SourceCurrencyTextBox.Text) || !string.IsNullOrEmpty(SourceCurrencyTextBox.Text))
                // 获取用户输入的美元数额
                sourceCurrencyText = SourceCurrencyTextBox.Text;

            if (decimal.TryParse(sourceCurrencyText, out decimal usdAmount))
            {
                try
                {
                    string sourceCurrency = "USD";
                    string targetCurrency = "CNY";
                    if (SourceCurrencyComboBox != null && SourceCurrencyComboBox.SelectedItem is ComboBoxItem selectedSourceItem)
                    {
                        // 获取 ComboBoxItem 的 Content
                        sourceCurrency = selectedSourceItem.Content.ToString();
                    }
                    if (TargetCurrencyComboBox != null && TargetCurrencyComboBox.SelectedItem is ComboBoxItem selectedItem)
                    {
                        // 获取 ComboBoxItem 的 Content
                        targetCurrency = selectedItem.Content.ToString();
                    }


                    // 请求汇率数据
                    decimal conversionRate = 0;//await CurrencyServices.GetConversionRate(sourceCurrency, targetCurrency);

                    // 计算人民币结果
                    decimal rmbResult = usdAmount * conversionRate;

                    // 将结果显示到RMBResultTextBox
                    RMBResultTextBox.Text = rmbResult.ToString("F2");
                }
                catch (Exception ex)
                {
                    RMBResultTextBox.Text = "Error: " + ex.Message;
                }
            }
            else
            {
                RMBResultTextBox.Text = "Invalid input!";
            }
            ProgressBar1.IsIndeterminate = false;
        }

        private void SourceCurrencyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SourceCurrencyComboBox.SelectedIndex != 0)
            {
                if (SourceCurrencyComboBox != null && SourceCurrencyComboBox.SelectedItem is ComboBoxItem selectedSourceItem)
                {
                    // 获取 ComboBoxItem 的 Content
                    localSettings.Values["currencyFrom"] = selectedSourceItem.Content.ToString();
                }
            }
            else
            {
                localSettings.Values.Remove("currencyFrom");
            }
        }

        private void TargetCurrencyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TargetCurrencyComboBox.SelectedIndex != 0)
            {
                if (TargetCurrencyComboBox != null && TargetCurrencyComboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    // 获取 ComboBoxItem 的 Content
                    localSettings.Values["currencyTo"] = selectedItem.Content.ToString();
                }
            }
            else
            {
                localSettings.Values.Remove("currencyTo");
            }
        }

        private void SelectComboBoxItemByContent(ComboBox comboBox, string content)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                // 检查每一项是否为 ComboBoxItem
                if (comboBox.Items[i] is ComboBoxItem item && item.Content.ToString() == content)
                {
                    // 匹配到指定的 Content
                    comboBox.SelectedIndex = i;
                    return;
                }
            }

            // 如果没有找到匹配的内容
            Debug.WriteLine("No matching content found.");
        }
    }
}

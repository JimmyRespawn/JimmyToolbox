using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace JimmyToolbox.Services
{
    public class CurrencyServices
    {
        public static async Task<decimal> GetConversionRate(string sourceCurrency, string targetCurrency)
        {
            string apiUrl = string.Format("https://api.frankfurter.dev/v1/latest?base={0}&symbols={1}", sourceCurrency, targetCurrency);

            using (HttpClient client = new HttpClient())
            {
                // 发送GET请求
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                // 确保请求成功
                response.EnsureSuccessStatusCode();

                // 读取响应内容
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // 解析JSON数据
                JObject json = JObject.Parse(jsonResponse);

                // 提取汇率
                decimal conversionRate = json["rates"][targetCurrency].Value<decimal>();
                return conversionRate;
            }
        }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Networking;
using Windows.Storage.Streams;

namespace JimmyToolbox.Services
{
    public class WakeOnLanService
    {
        public static async Task<bool> SendWakeOnLan(string macAddress, string broadcastIP = "255.255.255.255", int port = 9)
        {
            try
            {
                // 验证MAC地址
                if (string.IsNullOrWhiteSpace(macAddress) || !IsValidMacAddress(macAddress))
                    throw new ArgumentException("无效的MAC地址格式");

                // 构造魔术包
                byte[] magicPacket = BuildMagicPacket(macAddress);

                // 设置广播地址和端口
                HostName host = new HostName(broadcastIP);

                // 使用DatagramSocket发送数据
                using (DatagramSocket socket = new DatagramSocket())
                {
                    // 数据写入输出流
                    using (IOutputStream outputStream = await socket.GetOutputStreamAsync(host, port.ToString()))
                    {
                        DataWriter writer = new DataWriter(outputStream);
                        writer.WriteBytes(magicPacket);
                        await writer.StoreAsync();
                    }
                }

                System.Diagnostics.Debug.WriteLine("Wake-on-LAN 魔术包已发送");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"发送失败: {ex.Message}");
            }
            return false;
        }

        private static bool IsValidMacAddress(string macAddress)
        {
            // 验证MAC地址格式 (支持 `:` 或 `-` 作为分隔符)
            return System.Text.RegularExpressions.Regex.IsMatch(macAddress, @"^([0-9A-Fa-f]{2}[-:]){5}([0-9A-Fa-f]{2})$");
        }

        private static byte[] BuildMagicPacket(string macAddress)
        {
            // 去掉分隔符
            byte[] macBytes = macAddress
                .Split(new[] { ':', '-' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(hex => Convert.ToByte(hex, 16))
                .ToArray();

            // 构建魔术包: 6字节0xFF + 16次MAC地址
            byte[] magicPacket = new byte[6 + 16 * macBytes.Length];
            for (int i = 0; i < 6; i++) magicPacket[i] = 0xFF; // 前6字节填充0xFF
            for (int i = 0; i < 16; i++) System.Buffer.BlockCopy(macBytes, 0, magicPacket, 6 + i * macBytes.Length, macBytes.Length);

            return magicPacket;
        }
    }
}

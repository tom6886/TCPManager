using System;
using System.Net;
using System.Text;
using TCPHandler;

namespace TCPServer
{
    class Program
    {
        static SocketListener listener;

        static void Main(string[] args)
        {
            string com = "";
            do
            {
                com = Console.ReadLine().ToUpper();

                switch (com)
                {
                    case "TCP":
                        try
                        {
                            listener = new SocketListener(200, 1024);

                            listener.Init();

                            listener.Start(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 13909));

                            listener.OnClientNumberChange += Listener_OnClientNumberChange;

                            listener.GetIDByEndPoint += Listener_GetIDByEndPoint;

                            listener.GetPackageLength += Listener_GetPackageLength;

                            listener.OnMsgReceived += Listener_OnMsgReceived;

                            listener.GetSendMessage += Listener_GetSendMessage;

                            listener.OnSended += Listener_OnSended;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(string.Format("发生错误：{0}", ex.Message));
                        }
                        break;
                    case "UID":
                        try
                        {
                            if (listener == null) { Console.WriteLine("请先初始化TCP服务"); break; }

                            string[] uids = listener.OnlineUID;

                            Console.WriteLine(String.Join(",", uids));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(string.Format("发生错误：{0}", ex.Message));
                        }

                        break;
                    default:
                        Console.WriteLine("无法识别的命令");
                        break;
                }

            } while (com.ToUpper() != "EXIT");
        }

        private static void Listener_OnSended(AsyncUserToken token, System.Net.Sockets.SocketError error)
        {
            Console.WriteLine("已回馈消息：");
            Console.WriteLine(" 来源IP：" + token.Remote.Address.ToString());
        }

        private static byte[] Listener_GetSendMessage(string msg)
        {
            byte[] sendBuffer = Encoding.UTF8.GetBytes(msg);

            byte[] buff = new byte[sendBuffer.Length + 4];
            Array.Copy(BitConverter.GetBytes(sendBuffer.Length), buff, 4);
            Array.Copy(sendBuffer, 0, buff, 4, sendBuffer.Length);

            return buff;
        }

        private static void Listener_OnMsgReceived(AsyncUserToken token, byte[] info)
        {
            Console.WriteLine("接收到数据：");
            Console.WriteLine(" 来源IP：" + token.Remote.Address.ToString());
            Console.WriteLine(" 连接时间：" + token.ConnectTime.ToString());
            Console.WriteLine(" 内容：" + Encoding.UTF8.GetString(info));

            listener.Send(token.UID, "已接收到消息：" + Encoding.UTF8.GetString(info));
        }

        private static void Listener_OnClientNumberChange(int number, AsyncUserToken token)
        {
            Console.WriteLine(number > 0 ? "有设备接入" : "有设备断开");
            Console.WriteLine(" 来源IP：" + token.Remote.Address.ToString());
            Console.WriteLine(" 连接时间：" + token.ConnectTime.ToString());
        }

        private static int Listener_GetPackageLength(byte[] data, out int headLength)
        {
            headLength = 4;
            byte[] lenBytes = new byte[4];
            Array.Copy(data, lenBytes, 4);
            int packageLen = BitConverter.ToInt32(lenBytes, 0);
            return packageLen;
        }

        private static string Listener_GetIDByEndPoint(IPEndPoint endPoint)
        {
            return endPoint.GetHashCode().ToString();
        }
    }
}

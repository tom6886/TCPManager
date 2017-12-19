using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    class Program
    {
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
                            SocketConfig config = new SocketConfig()
                            {
                                MaxConnectCount = 200,
                                MaxBufferSize = 1024,
                                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 13909)
                            };

                            SocketManager m_socket = new SocketManager(config);

                            m_socket.Init();

                            m_socket.Start();

                            m_socket.ReceiveClientData += M_socket_ReceiveClientData;

                            m_socket.ClientNumberChange += M_socket_ClientNumberChange;

                            Console.WriteLine("服务启动");
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

        private static void M_socket_ClientNumberChange(int num, AsyncUserToken token)
        {
            Console.WriteLine(num > 0 ? "有设备接入" : "有设备断开");
            Console.WriteLine(" 来源IP：" + token.IPAddress.ToString());
            Console.WriteLine(" 连接时间：" + token.ConnectTime.ToString());
        }

        private static void M_socket_ReceiveClientData(AsyncUserToken token, byte[] buff)
        {
            Console.WriteLine("接收到数据：");
            Console.WriteLine(" 来源IP：" + token.IPAddress.ToString());
            Console.WriteLine(" 连接时间：" + token.ConnectTime.ToString());
            Console.WriteLine(" 内容：" + Encoding.UTF8.GetString(buff));
        }
    }
}

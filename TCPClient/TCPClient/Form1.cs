using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace TCPClient
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        static SocketManager smanager = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(edit_ip.Text) || string.IsNullOrWhiteSpace(edit_port.Text)) { MessageBox.Show("请先填入IP和Port"); return; }

            SocketError res = Connect(edit_ip.Text, Convert.ToInt16(edit_port.Text));

            if (res == 0) { memoEdit1.MaskBox.AppendText("连接成功"); return; }

            memoEdit1.MaskBox.AppendText("连接失败，错误码：" + res);
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(memoEdit2.Text)) { MessageBox.Show("请输入发送内容"); return; }

            Send(memoEdit2.Text);
        }

        #region 属性
        public static bool Connected
        {
            get { return smanager != null && smanager.Connected; }
        }

        #endregion

        #region 方法

        public static SocketError Connect(string ip, int port)
        {
            if (Connected) return SocketError.Success;

            if (string.IsNullOrWhiteSpace(ip) || port <= 1000) return SocketError.Fault;

            //创建连接对象, 连接到服务器  
            smanager = new SocketManager(ip, port);
            SocketError error = smanager.Connect();
            if (error == SocketError.Success)
            {
                //连接成功后,就注册事件. 最好在成功后再注册.  
                smanager.ServerDataHandler += OnReceivedServerData;
                smanager.ServerStopEvent += OnServerStopEvent;
            }
            return error;
        }

        /// <summary>  
        /// 发送消息  
        /// </summary>  
        /// <param name="message">消息实体</param>  
        /// <returns>True.已发送; False.未发送</returns>  
        public static bool Send(string message)
        {
            if (!Connected) return false;

            byte[] buff = Encoding.UTF8.GetBytes(message);
            //加密,根据自己的需要可以考虑把消息加密  
            //buff = AESEncrypt.Encrypt(buff, m_aesKey);  
            smanager.Send(buff);
            return true;
        }


        /// <summary>  
        /// 发送字节流  
        /// </summary>  
        /// <param name="buff"></param>  
        /// <returns></returns>  
        static bool Send(byte[] buff)
        {
            if (!Connected) return false;
            smanager.Send(buff);
            return true;
        }

        /// <summary>  
        /// 接收消息  
        /// </summary>  
        /// <param name="buff"></param>  
        private static void OnReceivedServerData(byte[] buff)
        {

        }

        /// <summary>  
        /// 服务器已断开  
        /// </summary>  
        private static void OnServerStopEvent()
        {

        }
        #endregion
    }
}

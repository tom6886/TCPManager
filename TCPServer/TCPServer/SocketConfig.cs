using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    public class SocketConfig
    {
        /// <summary>
        /// 最大连接数
        /// </summary>
        public int MaxConnectCount { get; set; }

        /// <summary>
        /// 最大接收字节数
        /// </summary>
        public int MaxBufferSize { get; set; }

        /// <summary>
        /// 分配缓冲区的读写倍数，默认为2
        /// </summary>
        public int OpsToAlloc { get { return opsToAlloc; } set { opsToAlloc = value; } }

        private int opsToAlloc = 2;

        /// <summary>
        /// 由缓冲池控制的总字节数,计算方式：最大连接数*最大接收字节数*分配缓冲区的读写倍数
        /// </summary>
        public int MaxBytesNumber { get { return MaxBufferSize * MaxConnectCount * OpsToAlloc; } }

        /// <summary>
        /// 服务地址
        /// </summary>
        public IPEndPoint EndPoint { get; set; }
    }
}

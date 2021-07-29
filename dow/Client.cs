using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using dow;

namespace Client
{
    public class Client
    {
        private Socket client;
        public bool isRead = false;
        private Queue<SendPkg> sendPkgs = new Queue<SendPkg>();
        private ManualResetEvent ManualResetEvent;
        public void StartConnect(string hostName, int port)
        {
            Thread thread = new Thread(() =>
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var iPHostEntry = Dns.GetHostEntry(hostName);
                var iPAddress = iPHostEntry.AddressList[0];
                IPEndPoint endPoint = new IPEndPoint(iPAddress, port);
                client.Connect(endPoint);
                isRead = true;
                ManualResetEvent = new ManualResetEvent(true);
                Thread thread1 = new Thread(() =>
                {
                    SendPkg sendPkg = null;
                    while (true)
                    {
                        ManualResetEvent.WaitOne();
                        sendPkgs.TryPeek(out sendPkg);
                        if (sendPkg == null)
                        {
                            ManualResetEvent.Set();
                        }
                        else
                        {
                            switch (sendPkg.sendType)
                            {
                                case sendType.File:
                                    SendFile(sendPkg);
                                    break;
                                case sendType.massegs:
                                    SendMassgs(sendPkg);
                                    break;
                                case sendType.Null:
                                    break;
                                default:
                                    break;
                            }
                            //解析条件
                        }
                    }

                });
            });
            thread.Start();

        }
        public void SendMassgs(SendPkg sendPkg)
        {
            client.Send(Jons.ToJsonToByte<SendPkg>(sendPkg));
        }
        public void SendFile(SendPkg sendPkg)
        {
            client.SendFile(sendPkg.Additionalinformation);
        }
        
    }
    public class SendPkg
    {
        public sendType sendType;
        /// <summary>
        /// 额外信息
        /// </summary>
        public string Additionalinformation;
        public MassegsPkg MassegsPkg = null;
        /// <summary>
        /// 创建数据
        /// </summary>
        /// <param name="type">发送类型</param>
        /// <param name="data">额外信息</param>
        /// <param name="massegsPkg">发送的包</param>
        public SendPkg(sendType type, string data, MassegsPkg massegsPkg)
        {
            sendType = type;
            this.Additionalinformation = data;
            this.MassegsPkg = massegsPkg;
        }

    }

    /// <summary>
    /// 自定义包 你的数据
    /// </summary>
    public class MassegsPkg
    {

    }
    public enum sendType
    {
        File = 1,
        massegs = 2,
        Null = 0
    }
}

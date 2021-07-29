using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
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
                                    break;
                                case sendType.strings:
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
        public void SendMassgs(string massgs)
        {

        }
        private void SendFile(string path)
        {
            client.SendFile(path);
        }
        private void SendMassgs(SendPkg data)
        {

            
        }
    }
    public class SendPkg
    {
        public sendType sendType;
        public object data;
        public SendPkg(sendType type, object data)
        {
            sendType = type;
            this.data = data;
        }

    }
    public enum sendType
    {
        File,
        strings,
        Null
    }
}

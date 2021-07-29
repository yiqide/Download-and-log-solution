using dow;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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
            
            var iPHostEntry = Dns.GetHostEntry(hostName);
            var iPAddress = iPHostEntry.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(iPAddress, port);
            client = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(endPoint);
            isRead = true;
            ManualResetEvent = new ManualResetEvent(true);
            Thread thread1 = new Thread(() =>
            {
                SendPkg sendPkg = null;
                while (true)
                {
                    Console.WriteLine("131");
                    ManualResetEvent.WaitOne();
                    sendPkgs.TryPeek(out sendPkg);
                    if (sendPkg == null)
                    {
                        ManualResetEvent.Reset();
                    }
                    else
                    {
                        switch (sendPkg.sendType)
                        {
                            case sendType.massegs:
                                SendMassgs(sendPkg.MassegsPkg);
                                break;
                            case sendType.Null:
                                break;
                        }
                        Console.WriteLine("发送了消息");
                        sendPkgs.Dequeue();

                    }
                }

            });
            thread1.Start();
        });
        thread.Start();

    }
    private void SendMassgs(MassegsPkg sendPkg)
    {
        client.Send(Jons.ToJsonToByte<MassegsPkg>(sendPkg));
    }
    public void AddTask(SendPkg pkg) 
    {
        sendPkgs.Enqueue(pkg);
        ManualResetEvent.Set();
    }

}
public class SendPkg
{
    public sendType sendType;
    public MassegsPkg MassegsPkg = null;
    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="type">发送类型</param>
    /// <param name="data">额外信息</param>
    /// <param name="massegsPkg">发送的包</param>
    public SendPkg(sendType type, MassegsPkg massegsPkg)
    {
        sendType = type;
        this.MassegsPkg = massegsPkg;
    }

}

/// <summary>
/// 自定义包 你的数据
/// </summary>
public class MassegsPkg
{
    public string massegs { get; set; }
    public MassegsPkg(string massegs) 
    {
        this.massegs = massegs;
    }
}
public enum sendType
{
    massegs = 1,
    Null = 0
}


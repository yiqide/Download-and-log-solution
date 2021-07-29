using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

internal class 服务器
{
    public Socket socket;
    public List<Socket> clients = new List<Socket>();
    private IPEndPoint iPEndPoint;
    private Queue<Data> massges = new Queue<Data>();
    public 服务器(string 域名, int port)
    {
        IPHostEntry iPHostEntry = Dns.GetHostEntry(域名);
        IPAddress iPAddress = iPHostEntry.AddressList[0];
        iPEndPoint = new IPEndPoint(iPAddress, port);
    }
    public void Start()
    {
        socket.Bind(iPEndPoint);
        socket.Listen(100);
        Thread thread = new Thread(启动);
        thread.Start();
    }

    private void 启动()
    {
        while (true)
        {
            Socket client = socket.Accept();
            clients.Add(client);
            Thread thread = new Thread(连接);
            thread.Start(client);
        }
    }
    private void 连接(object client)
    {
        Socket c = (Socket)client;
        string data = "";
        while (true)
        {
            while (true)
            {
                byte[] buff = new byte[1024];
                int count = c.Receive(buff);
                data += Encoding.UTF8.GetString(buff, 0, count);
                if (data.IndexOf("<EOF>") > -1)
                {
                    //出现结束标记
                    break;
                }
            }
            massges.Enqueue(new Data(c, data)); //传递到消息队列
            data = "";
        }
    }

    public struct Data
    {
        private Socket client;
        private object obj;
        public Data(Socket client, object data)
        {
            this.client = client;
            obj = data;
        }
    }
}


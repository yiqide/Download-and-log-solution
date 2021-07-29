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
        Thread thread1 = new Thread(监听消息);
        thread1.Start();
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
                if (c.Available<=0)
                {
                    //出现结束标记
                    break;
                }
            }
            Addtask(new Data(c, data)); //传递到消息队列
            data = "";
        }
    }
    private ManualResetEvent ManualResetEvent=new ManualResetEvent(true);
    private void Addtask(Data data) 
    {
        massges.Enqueue(data);
        ManualResetEvent.Set();//放行
    }
    private void 监听消息() 
    {
        Data data=null;
        while (true)
        {
            ManualResetEvent.WaitOne();
            massges.TryPeek(out data);
            if (data == null)
            {
                处理消息(data.client, data.obj);
                massges.Dequeue();
            }
            else ManualResetEvent.Reset();


        }
    }
    private void 处理消息(Socket client,string data) 
    {
        Console.WriteLine(client.LocalEndPoint+":"+data);
    }




    public class Data
    {
        public Socket client;
        public string obj;
        public Data(Socket client, string data)
        {
            this.client = client;
            obj = data;
        }
    }
}


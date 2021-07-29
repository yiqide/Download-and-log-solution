using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections;
using System.Text.Unicode;
using System.Text.Json.Serialization;

namespace dow
{
    class Program
    {
        static void Main(string[] args)
        {
            //服务器 server = new 服务器(6666);
            //server.Start();
            Client client = new Client();
            client.StartConnect("dujiaoshou.store", 6666);
            while (true)
            {
                string s = Console.ReadLine();
                client.AddTask(new SendPkg(sendType.massegs, new MassegsPkg(s)));
            }

        }
    }
            
}




using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace dow
{
    /// <summary>
    /// 日志系统
    /// 使用方法：new 一个日志
    /// 然后使用addLog()就可以了
    /// </summary>
    public class 日志
    {
        private StreamWriter streamWriter;
        private Thread thread;
        private string path;
        private string fileName;
        private ManualResetEvent ManualResetEvent = new ManualResetEvent(true);
        private void init(string path, string fileName)
        {
            queue = new Queue<byte[]>();
            string p = path + "/" + fileName;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(p))
            {
                var f = File.Create(p);
                f.Close();
                f.Dispose();
            }
            streamWriter = File.AppendText(p);
            Console.WriteLine(p);
            thread = new Thread(() =>
            {
                byte[] vs;
                while (true)
                {
                    ManualResetEvent.WaitOne();
                    queue.TryPeek(out vs);
                    if (vs != null)
                    {
                        streamWriter.WriteLine(UnicodeEncoding.UTF8.GetString(vs));
                        streamWriter.Flush();
                        queue.Dequeue();
                    }
                    vs = null;
                    queue.TryPeek(out vs);
                    if (vs == null)
                    {
                        ManualResetEvent.Reset();//停止线程

                    }
                }
            });
            thread.Start();
        }
        private Queue<byte[]> queue;
        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="data"></param>
        public void addLog(string data)
        {
            if (thread == null) init(path, fileName);
            queue.Enqueue(UnicodeEncoding.UTF8.GetBytes(data));
            //如果线程没有启动就启动线程
            ManualResetEvent.Set();
        }
        public void addLog(byte[] data)
        {
            if (thread == null) init(path, fileName);
            queue.Enqueue(data);
            //如果线程没有启动就启动线程
            ManualResetEvent.Set();
        }
        public 日志(string path, string fileName)
        {
            this.path = path;
            this.fileName = fileName;
        }
    }
    
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections;

namespace dow
{
    class Program
    {
        static void Main(string[] args)
        {
            //string path= @"D:\txt.txt";
            //string url = @"http://downloadg.dewmobile.net/official/ZapyaPC2803Lite.exe";
            string filepath = @"E:\桌面\";
            string fileName = "Bili.txt";
            //var a= DowHanld.CorectTask(url,filepath+fileName,"t1");
            //a.Start();
            ////LocalLog.init(filepath, fileName);
            //while (true)
            //{
            //    Console.WriteLine("大小："+ a.DowDateSzie+"速度："+a.speed);
            //    Thread.CurrentThread.Join(1000);
            //    //LocalLog.addLog( Console.ReadLine());
            //}
            LocalLog.init(filepath,fileName);
            while (true)
            {
                LocalLog.addLog( Console.ReadLine());
            }
           
            
        }
    }
    /// <summary>
    /// 异步下载任务系统下载解决方案
    /// </summary>
    public static class DowHanld
    {
        private static List<DowTask> dowTaskList_NotStarted = new List<DowTask>();
        private static List<DowTask> dowTaskList_UnderWay = new List<DowTask>();
        private static List<DowTask> dowTaskList_Done = new List<DowTask>();
        private static List<DowTask> dowTaskList_Defeat = new List<DowTask>();

        public static DowTask CorectTask(string url, string path, string taskName)
        {
            return new DowTask(path, url, taskName);
        }

        public static List<DowTask> DowTasks_NotStarte
        {
            get { return dowTaskList_NotStarted; }
        }

        public static List<DowTask> DowTaskList_UnderWay
        {
            get { return dowTaskList_UnderWay; }
        }

        public static List<DowTask> DowTaskList_Done
        {
            get { return dowTaskList_Done; }
        }

        public static List<DowTask> DowTaskList_Defeat
        {
            get { return dowTaskList_Defeat; }
        }


        public class DowTask
        {
            public string path;
            public string url;
            private DowState state;
            public string name = "";
            public DowState State
            {
                get { return state; }
                private set
                {
                    switch (value)
                    {
                        case DowState.Defeat:
                            RemoveOdlState(state);
                            break;
                        case DowState.Done:
                            RemoveOdlState(state);
                            break;
                        case DowState.NotStarted:
                            RemoveOdlState(state);
                            break;
                        case DowState.UnderWay:
                            RemoveOdlState(state);
                            break;
                    }
                    state = value;
                }
            }

            private void RemoveOdlState(DowState value)
            {
                switch (value)
                {
                    case DowState.NotStarted:
                        dowTaskList_NotStarted.Remove(this);
                        break;
                    case DowState.UnderWay:
                        dowTaskList_UnderWay.Remove(this);
                        break;
                    case DowState.Done:
                        dowTaskList_Done.Remove(this);
                        break;
                    case DowState.Defeat:
                        dowTaskList_Defeat.Remove(this);
                        break;
                    default:
                        return;
                }
            }


            public float DowDateSzie = 0;
            public float speed;
            private Thread thread;


            public void GoOn() 
            {

            }
            /// <summary>
            /// 暂停
            /// </summary>
            public void Stop() 
            {
                if (thread != null) thread.Join();

            }


            public void Start()
            {
                State = DowState.UnderWay;
                thread = new Thread(() =>{
                    DownloadAssetAsync(url, path);
                });
                Thread thread2 = new Thread(()=> 
                {
                    float star = DowDateSzie;
                    while (true)
                    {
                        speed= DowDateSzie - star;
                        star = DowDateSzie;
                        Thread.CurrentThread.Join(1000);
                    }
                });
                thread2.Start();
                thread.Start();
            }
            private void DownloadAssetAsync(string url, string filePath)
            {
                try
                {
                    WebRequest Myrq = HttpWebRequest.Create(url);
                    WebResponse myrp = Myrq.GetResponse();
                    Stream st = myrp.GetResponseStream();
                    Stream so = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
                    byte[] by = new byte[1024 * 16];
                    int osize = st.Read(by, 0, (int)by.Length);
                    int count = 0;
                    while (osize > 0)
                    {
                        so.Write(by, 0, osize);
                        osize = st.Read(by, 0, (int)by.Length);
                        count++;
                        DowDateSzie = count * (float)16 / 1024;

                    }
                    so.Close();
                    so.Dispose();
                    st.Close();
                    st.Dispose();
                    myrp.Close();
                    myrp.Dispose();
                    Myrq.Abort();
                    State = DowState.Done;
                }
                catch (System.Exception e)
                {
                    State = DowState.Defeat;
                    throw e;
                }
            }

            public DowTask(string path, string url, string name)
            {
                this.path = path;
                this.url = url;
                this.name = name;
                state = DowState.NotStarted;
            }
        }

        public enum DowState
        {
            NotStarted,
            UnderWay,
            Done,
            Defeat
        }
    }

    public static class FileHanldAsync
    {
        public static async Task WritedFileAsync(string path, string data)
        {
            FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
            byte[] bytes = UnicodeEncoding.UTF8.GetBytes(data);
            await fileStream.WriteAsync(bytes, 0, bytes.Length);
            fileStream.Close();
            fileStream.Dispose();
        }

        public static async Task WriedFileAsync(string path, byte[] data)
        {
            FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
            await fileStream.WriteAsync(data, 0, data.Length);
            fileStream.Close();
            fileStream.Dispose();
        }

        //public static async Task WriteAppendAsync(string path, string data)
        //{
        //    if (!File.Exists(path))
        //    {
        //        File.Create(path).Dispose();
        //    }
        //    if (FileHanldAsync.fileStream == null)
        //    {
        //        fileStream = new FileStream(path, FileMode.Append);
        //    }
        //    data = System.Environment.NewLine + data;
        //    byte[] bytes = UnicodeEncoding.UTF8.GetBytes(data);
        //    await fileStream.WriteAsync(bytes, 0, bytes.Length);
        //}

       
    }
}
/// <summary>
/// 日志系统
/// </summary>
public static class LocalLog
{
    private static StreamWriter streamWriter;
    /// <summary>
    /// 初始化日志系统
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="fileName">文件名字</param>>

    private static Thread thread;
    public static void init(string path ,string fileName)
    {
        queue = new Queue<byte[]>();
        string p = path + @"\" + fileName;
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
        thread = new Thread(() =>
        {
            while (true)
            {
                byte[] vs;
                queue.TryPeek(out vs);
                if (vs != null)
                {
                    streamWriter.WriteLine(UnicodeEncoding.UTF8.GetString(vs));
                    streamWriter.Flush();
                    queue.Dequeue();
                    Console.WriteLine("??");
                }
                vs = null;
                queue.TryPeek(out vs);
                if (vs == null)
                {
                    Console.WriteLine("停止");
                    Thread.Sleep(Timeout.Infinite);

                }
            }
        });
        thread.Start();
    }
    private static Queue<byte[]> queue ;
    private static void WriteLog() 
    {
        byte[] vs;
        queue.TryPeek(out vs);
        if (vs != null)
        {
            streamWriter.WriteLine(UnicodeEncoding.UTF8.GetString(vs));
            streamWriter.Flush();
            queue.Dequeue();
            Console.WriteLine("??");
        }
        vs = null;
        queue.TryPeek(out vs);
        if (vs==null) 
        {
            Console.WriteLine("停止");
            Thread.Sleep(Timeout.Infinite);
            
        }
    }

    /// <summary>
    /// 保存日志
    /// </summary>
    /// <param name="data"></param>
    public static void addLog(string data)
    {
        queue.Enqueue(UnicodeEncoding.UTF8.GetBytes(data));
        Console.WriteLine(thread.ThreadState.ToString());
        if (thread.ThreadState==ThreadState.WaitSleepJoin||thread.ThreadState==ThreadState.Unstarted) thread.Interrupt();
        Console.WriteLine(thread.ThreadState.ToString());

    }
}


public static class jons
{
    public static void dsa() 
    {
        
        WaitHandleCannotBeOpenedException waitHandleCannotBeOpenedException = new WaitHandleCannotBeOpenedException();
        waitHandleCannotBeOpenedException.Source = "你好";
        byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(waitHandleCannotBeOpenedException);
        Console.WriteLine(UnicodeEncoding.UTF8.GetString( jsonUtf8Bytes));


        var readOnlySpan = new ReadOnlySpan<byte >(jsonUtf8Bytes);
        WaitHandleCannotBeOpenedException deserializedWeatherForecast =
            JsonSerializer.Deserialize<WaitHandleCannotBeOpenedException>(readOnlySpan);

        Console.WriteLine(deserializedWeatherForecast.Source);
    }
}
public class WeatherForecastWithPOCOs
{
    public DateTimeOffset Date { get; set; }
    public int TemperatureCelsius { get; set; }
    public string Summary { get; set; }
    public string SummaryField;
    public IList<DateTimeOffset> DatesAvailable { get; set; }
    public Dictionary<string, HighLowTemps> TemperatureRanges { get; set; }
    public string[] SummaryWords { get; set; }
}

public class HighLowTemps
{
    public int High { get; set; }
    public int Low { get; set; }
}


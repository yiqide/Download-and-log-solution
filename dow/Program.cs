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
            //ManualResetEvent manualResetEvent = new ManualResetEvent(true);
            //Thread thread = new Thread(()=> {
            //    while (true)
            //    {
            //        manualResetEvent.WaitOne();
            //        Thread.Sleep(250);
            //        Console.WriteLine("********");
            //    }

            //});
            //thread.IsBackground=true;
            //thread.Start();
            //while (true)
            //{

            //    if ("1" == Console.ReadLine())
            //    {
            //        Console.WriteLine("放行");
            //        manualResetEvent.Set();
            //    }
            //    else 
            //    {
            //        Console.WriteLine("不放行");
            //        manualResetEvent.Reset();
            //    }
            //    Console.WriteLine(thread.ThreadState.ToString());
            //}



        }
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


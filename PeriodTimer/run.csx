using System;

public static void Run(TimerInfo myTimer, TraceWriter log)
{
    var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    log.Info($"C# Timer trigger function executed at: {now}");    
}

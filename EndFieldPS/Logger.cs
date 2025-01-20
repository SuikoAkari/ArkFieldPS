using System;
using System.IO;

public static class Logger
{
    private static StreamWriter logWriter;
    private static bool hideLogs;

    public static void Initialize(bool hideLogs = false)
    {
        Logger.hideLogs = hideLogs;
        logWriter = new StreamWriter("latest.log", false);
    }

    public static void Log(string message)
    {
        if (!hideLogs)
        {
            try
            {
                logWriter.WriteLine($"{DateTime.Now}: {message}");
                logWriter.Flush();
            }
            catch(Exception e)
            {

            }
           
        }
    }

    public static void Close()
    {
        logWriter.Close();
    }
}
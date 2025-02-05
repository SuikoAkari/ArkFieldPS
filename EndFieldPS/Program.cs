// See https://aka.ms/new-console-template for more information
using EndFieldPS;
using Newtonsoft.Json;

class Program
{
    static void Main(string[] args)
    {
        StartServer(args);
    }

    private static void StartServer(string[] args)
    {
        Console.Title = "Initializing...";

        bool disableLogs = args.Length > 0 && args[0].ToLower() == "nologs";

        ConfigFile config = new ConfigFile();
        if (File.Exists("server_config.json"))
        {
            config = JsonConvert.DeserializeObject<ConfigFile>(File.ReadAllText("server_config.json"))!;
        }
        File.WriteAllText("server_config.json", JsonConvert.SerializeObject(config, Formatting.Indented));

        new Thread(() =>
        {
            new Server().Start(disableLogs, config);
        }).Start();
        AppDomain.CurrentDomain.ProcessExit += (_, _) =>
        {
            Console.WriteLine("Shutting down...");
            
            Server.Shutdown();
        };

        while (Server.Initialized == false)
        {

        }
        Console.Title = $"EndFieldPS Server v{Server.ServerVersion}";
    }
}
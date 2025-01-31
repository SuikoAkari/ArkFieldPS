using EndFieldPS.Commands;
using EndFieldPS.Commands.Handlers;
using EndFieldPS.Database;
using HttpServerLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace EndFieldPS.Http
{
    public class PCDispatch
    {
        //using as console for player
        [StaticRoute(HttpServerLite.HttpMethod.GET, "/pcSdk/userInfo")]
        public static async Task Info(HttpContext ctx)
        {
            string resp = File.ReadAllText("Data/PlayerConsole/index.html").Replace("%dispatchip%", $"http://{Server.config.dispatchServer.bindAddress}:{Server.config.dispatchServer.bindPort}");

            ctx.Response.StatusCode = 200;
            ctx.Response.ContentType = "text/html";

            await ctx.Response.SendAsync(resp);
        }



        [StaticRoute(HttpServerLite.HttpMethod.GET, "/pcSdk/console")]
        public static async Task ConsoleResponce(HttpContext ctx)
        {
            string[] playerCommands = ["help", "heal", "scene", "spawn"];
            string command = ctx.Request.Query.Elements["command"];
            string token = ctx.Request.Query.Elements["token"];
            string message = "Unknown command";
            var args = command.Split('+').Skip(1).ToArray();

            if (token != null)
            {
                Account account = DatabaseManager.db.GetAccountByToken(token);
                Logger.Print(account.id);
                Player player = Server.clients.Find(acc => acc.accountId == account.id);


                switch (command.Split('+').ToArray()[0])
                {
                    case "help":
                        message = "";
                        foreach (var com in CommandManager.s_notifyReqGroup)
                        {
                            if(playerCommands.Contains(com.Key))
                            {
                                message += $"\n{com.Key} - {com.Value.Item1.desc}";
                            }
                        }
                        break;

                    case "heal":
                        CommandHeal.HealCmd("", [], player);
                        message = "Healed";
                        break;

                    case "scene":
                        BaseCommands.SceneCmd("", args, player);
                        message = "Scene changed";
                        break;

                    case "spawn":
                        CommandSpawn.SpawnCmd("", args, player);
                        message = $"Enemy with id {args[0]} was spawned";
                        break;
                }
            }
            else message = "Token not found";

            var responseData = new
            {
                message = message,
            };

            string resp = System.Text.Json.JsonSerializer.Serialize(responseData);
            ctx.Response.Headers.Add("Content-Type", "application/json");
            ctx.Response.StatusCode = 200;
            await ctx.Response.SendAsync(resp);
        }
    }
}

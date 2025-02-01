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
            string cmd = ctx.Request.Query.Elements["command"].Replace("+"," ");
            string token = ctx.Request.Query.Elements["token"];
            string message = "";
            string[] split = cmd.Split(" ");
            string[] args = cmd.Split(" ").Skip(1).ToArray();
            string command = split[0].ToLower();
            if (token != null)
            {
                Account account = DatabaseManager.db.GetAccountByToken(token);
                if (account != null)
                {

                    Logger.Print(account.id);
                    Player player = Server.clients.Find(acc => acc.accountId == account.id);
                    if (player != null)
                    {

                        CommandManager.Notify(player, command, args, player);
                        foreach (string msg in player.temporanyChatMessages)
                        {
                            message += msg + "<br>";
                        }
                        player.temporanyChatMessages.Clear();
                    }
                    else
                    {
                        message = "You aren't connected to the server";
                    }
                }
                else
                {
                    message = "Account not found";
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

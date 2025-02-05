using EndFieldPS.Database;
using EndFieldPS.Resource;
using HttpServerLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Game.Gacha.GachaManager;
using static EndFieldPS.Http.Dispatch;

namespace EndFieldPS.Http
{
    public class SDK
    {
        [StaticRoute(HttpServerLite.HttpMethod.POST, "/user/auth/v1/token_by_email_password")]
        public static async Task token_login(HttpContext ctx)
        {
            string requestBody = ctx.Request.DataAsString;
            LoginJson body = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginJson>(requestBody);
            Account account = DatabaseManager.db.GetAccountByUsername(body.email.Split("@")[0]);
            Console.WriteLine(requestBody);
            string resp = "{}";
            if (account != null)
            {
                resp = "{\"msg\":\"OK\",\"status\":0,\"type\":\"A\",\"data\":{\"token\":\"" + account.token + "\"}}";
            }
            else
            {
                resp = "{\"msg\":\"Account not found\",\"status\":2,\"type\":\"A\"}";
            }

            ctx.Response.StatusCode = 200;
            //ctx.Response.ContentLength = resp.Length;
            ctx.Response.ContentType = "application/json";

            await ctx.Response.SendAsync(resp);

        }
        [StaticRoute(HttpServerLite.HttpMethod.GET, "/user/info/v1/basic")]
        public static async Task account_info_get(HttpContext ctx)
        {
            string requestToken = ctx.Request.Query.Elements["token"];
            Account account = DatabaseManager.db.GetAccountByToken(requestToken);
            string resp = "{\"data\":{\"hgId\":\"1799321925\",\"email\":\"dispatch@endfield.ps\",\"realEmail\":\"dispatch@endfield.ps\",\"isLatestUserAgreement\":true,\"nickName\":\"Endfield PS\"},\"msg\":\"OK\",\"status\":0,\"type\":1}";
            if (account != null)
            {
                resp = "{\"data\":{\"hgId\":\"" + account.id + "\",\"email\":\"" + account.username +Server.config.dispatchServer.emailFormat +"\",\"realEmail\":\"" + account.username + Server.config.dispatchServer.emailFormat + "\",\"isLatestUserAgreement\":true,\"nickName\":\"" + account.username + "\"},\"msg\":\"OK\",\"status\":0,\"type\":1}";
            }
            else
            {
                resp = "{\"msg\":\"Account not found\",\"status\":2,\"type\":\"A\"}";
            }




            ctx.Response.StatusCode = 200;
            //ctx.Response.ContentLength = resp.Length;
            ctx.Response.ContentType = "application/json";

            await ctx.Response.SendAsync(resp);
        }

        public struct GrantData
        {
            public string token;
        }
        [StaticRoute(HttpServerLite.HttpMethod.POST, "/user/oauth2/v2/grant")]
        public static async Task account_ugrant(HttpContext ctx)
        {
            string requestBody = ctx.Request.DataAsString;

            GrantData grant = Newtonsoft.Json.JsonConvert.DeserializeObject<GrantData>(requestBody);
            Account account = DatabaseManager.db.GetAccountByToken(grant.token);
            string resp = "{\"msg\": \"Error\",  \"status\": 2,  \"type\": \"A\"}";
            if (account != null)
            {
                resp = "{\"data\": {  \"uid\": \"" + account.id + "\",  \"code\": \"" + DatabaseManager.db.GrantCode(account) + "\"  },  \"msg\": \"OK\",  \"status\": 0,  \"type\": \"A\"}";
            }

            ctx.Response.StatusCode = 200;

            ctx.Response.ContentType = "application/json";

            await ctx.Response.SendAsync(resp);
        }
        [StaticRoute(HttpServerLite.HttpMethod.POST, "/u8/user/auth/v2/grant")]
        public static async Task account_grant(HttpContext ctx)
        {
            string requestBody = ctx.Request.DataAsString;

            GrantData grant = Newtonsoft.Json.JsonConvert.DeserializeObject<GrantData>(requestBody);
            Account account = DatabaseManager.db.GetAccountByTokenGrant(grant.token);
            string resp = "{\"msg\": \"Error\",  \"status\": 2,  \"type\": \"A\"}";
            if (account != null)
            {
                resp = "{\"data\": {  \"uid\": \"" + account.id + "\",  \"code\": \"" + account.grantToken + "\"  },  \"msg\": \"OK\",  \"status\": 0,  \"type\": \"A\"}";
            }

            ctx.Response.StatusCode = 200;

            ctx.Response.ContentType = "application/json";

            await ctx.Response.SendAsync(resp);
        }
        public class TokenChannelData
        {
            public string channelToken;

        }
        public class ChannelTokenData
        {
            public string code;
        }
        [StaticRoute(HttpServerLite.HttpMethod.POST, "/u8/user/auth/v2/token_by_channel_token")]
        public static async Task token_channel_token(HttpContext ctx)
        {
            try
            {
                string requestBody = ctx.Request.DataAsString;
                Console.WriteLine(requestBody);
                TokenChannelData data = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenChannelData>(requestBody);
                ChannelTokenData channelTokenBody = Newtonsoft.Json.JsonConvert.DeserializeObject<ChannelTokenData>(data.channelToken);
                string resp = "{  \"data\": {    \"token\":\"" + channelTokenBody.code + "\"  },  \"msg\": \"OK\",  \"status\": 0,  \"type\": \"\"}";

                ctx.Response.StatusCode = 200;

                ctx.Response.ContentType = "application/json";

                await ctx.Response.SendAsync(resp);
            }
            catch (Exception e)
            {
                Logger.PrintError(e.Message);
            }

        }
        /*{
            "appCode": "2fe67ec91610377d",
            "code": "121212",
            "email": "aaaa@a.cc",
            "from": 0,
            "password": "aaaaaaaaaaaaaa1"
        }*/
        public struct RegisterData
        {
            public string appCode;
            public string code;
            public string email;
            public string password;
        }
        [StaticRoute(HttpServerLite.HttpMethod.POST, "/user/auth/v1/register")]
        public static async Task register(HttpContext ctx)
        {
            try
            {
                string requestBody = ctx.Request.DataAsString;
                Console.WriteLine(requestBody);
                RegisterData data = Newtonsoft.Json.JsonConvert.DeserializeObject<RegisterData>(requestBody);
                string username = data.email.Split("@")[0];
                (string,int) msg=DatabaseManager.db.CreateAccount(username);
                string resp = "";
                if (msg.Item2 > 0)
                {
                    resp = "{\"msg\": \"" + msg.Item1 + "\",  \"status\": " + msg.Item2 + ",  \"type\": \"\"}";
                }
                else
                {
                    Account account = DatabaseManager.db.GetAccountByUsername(username);
                    resp = "{\"data\": {    \"token\":\"" + account.token + "\"  }, \"msg\": \"" + msg.Item1 + "\",  \"status\": " + msg.Item2 + ",  \"type\": \"\"}";
                }
                

                ctx.Response.StatusCode = 200;
                ctx.Response.ContentType = "application/json";

                await ctx.Response.SendAsync(resp);
            }
            catch (Exception e)
            {
                Logger.PrintError(e.Message);
            }

        }
        
        [StaticRoute(HttpServerLite.HttpMethod.GET, "/api/gachahistory")]
        public static async Task gachahistory_api(HttpContext ctx)
        {
            string requestId = ctx.Request.Query.Elements["id"];
            string banner = ctx.Request.Query.Elements["banner"];
            string page = ctx.Request.Query.Elements["page"];
            PlayerData data = DatabaseManager.db.GetPlayerById(requestId);
            GachaHistoryAPI transactions = new();
            if (data != null)
            {
                transactions = GetGachaHistoryPage(data, banner, int.Parse(page));
            }
            else
            {
                transactions.transactionList = new();
            }
            string resp = Newtonsoft.Json.JsonConvert.SerializeObject(transactions);

            ctx.Response.StatusCode = 200;
            await ctx.Response.SendAsync(resp);
        }
        [StaticRoute(HttpServerLite.HttpMethod.GET, "/gachahistory")]
        public static async Task gachahistory(HttpContext ctx)
        {
            string requestId = ctx.Request.Query.Elements["id"];

            PlayerData data = DatabaseManager.db.GetPlayerById(requestId);
            string resp = "";
            if (data != null)
            {
                resp = File.ReadAllText("Data/GachaHistory/index.html").Replace("%dispatchip%", $"http://{Server.config.dispatchServer.bindAddress}:{Server.config.dispatchServer.bindPort}");
            }
            else
            {
                resp = File.ReadAllText("Data/GachaHistory/index_noplayerfound.html");
            }




            ctx.Response.StatusCode = 200;

            await ctx.Response.SendAsync(resp);
        }
    }
}

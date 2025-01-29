# EndField PS
[EN](../README.md) | [IT](./README_it-IT.md) | [RU](./README_ru-RU.md) | [CN](./README_zh-CN.md) | [NL](./README_nl-NL.md)

![Logo](https://socialify.git.ci/SuikoAkari/EndFieldPS/image?custom_description=Private+server+for+EndField&amp;description=1&amp;font=Jost&amp;forks=1&amp;issues=1&amp;language=1&amp;logo=https%3A%2F%2Farknights.wiki.gg%2Fimages%2F3%2F31%2FArknights_Endfield_logo.png&amp;name=1&amp;pattern=Circuit+Board&amp;pulls=1&amp;stargazers=1&amp;theme=Dark)  

EndFieldPS 是一个为 《明日方舟：终末地》再次测试 提供的本地服务端项目。  

## 当前功能  

- 登录  
- 角色切换  
- 队伍切换  
- 场景切换（目前部分场景可用）  
- 使用 MongoDB 保存数据  

## 安装步骤  

1. 安装：
   * [.NET SDK](https://dotnet.microsoft.com/en-us/download)（推荐版本 8.0.12）
   * [MongoDB](https://www.mongodb.com/try/download/community) 
   * [Fiddler Classic](https://www.telerik.com/fiddler/fiddler-classic) 或 [mitmproxy](https://mitmproxy.org/)  

   1. 安装 *Fiddler Classic* 时，请确保启用了**解密 HTTPS 流量**并**安装证书**！  
   2. 需要通过菜单栏左上角的 Tools -> Options -> HTTPS，勾选 “Capture HTTPS CONNECTs” 和 “Decrypt HTTPS traffic”。您还可以通过 Actions（位于 “Capture HTTPS CONNECTs” 右侧）-> Trust Root Certificate 重新安装证书，点击 “Yes” 以确认。  

2. 下载 [预编译版本](https://github.com/SuikoAkari/EndFieldPS/releases/latest) 或自行构建项目。  
3. 将 `Json` 和 `TableCfg` 文件夹放入 `EndFieldPS.exe` 所在的目录（可以从 [这里](https://github.com/PotRooms/EndFieldData/tree/main) 下载副本）。  
4. 运行服务端（`EndFieldPS.exe`）。  
5. 覆盖 `C:\Users\<YourUserName>\Documents\Fiddler2\Scripts\CustomRules.js` 脚本（或者备份原有脚本并创建一个同名的新文件），替换为以下脚本：  
   - 您还可以运行 *Fiddler Classic*，转到 “Rules -> Customize Rules” (快捷键 CTRL + R) 保存脚本，或者通过 *FiddlerScript* 选项卡进行编辑。  

   ```javascript
   import System;
   import System.Windows.Forms;
   import Fiddler;
   import System.Text.RegularExpressions;

   class Handlers {
       static function OnBeforeRequest(oS: Session) {
           if (oS.host.Contains("gryphline.com") || oS.host.Contains("hg-cdn.com")) {
               if (oS.HTTPMethodIs("CONNECT")) {
                   return;
               }
               FiddlerObject.log(">>>>>>>>>>>> URL:" + oS.fullUrl);
               oS.oRequest.headers.UriScheme = "http";
               oS.oRequest["Cookie"] = (oS.oRequest["Cookie"] + ";OriginalHost=" + oS.host + ";OriginalUrl=" + oS.fullUrl);
               oS.host = "localhost:5000";
           }
       }
   };
   ```  

   或者使用以下 mitmproxy 命令：  

   ```shell
   mitmproxy -s ak.py
   ```  

   `ak.py` 脚本：  

   ```python
   import mitmproxy
   from mitmproxy import ctx, http

   class EndFieldModifier:
       def requestheaders(self, flow: mitmproxy.http.HTTPFlow):
           if "gryphline.com" in flow.request.host or "hg-cdn.com" in flow.request.host:
               if flow.request.method == "CONNECT":
                   return

               flow.request.scheme = "http"
               flow.request.cookies.update({
                   "OriginalHost": flow.request.host,
                   "OriginalUrl": flow.request.url
               })
               flow.request.host = "localhost"
               flow.request.port = 5000
               ctx.log.info("URL:" + flow.request.url)

   addons = [
       EndFieldModifier()
   ]
   ```  

6. 运行 *Fiddler Classic*，它应使用新的 *自定义规则脚本* 启动（您可以在 *FiddlerScript* 选项卡中查看）。  
7. 启动游戏客户端并开始游戏！（注意：目前仅支持国际服客户端）  
8. 您需要在服务端控制台中使用 `account create (用户名)` 创建账户，然后在游戏中以 `(用户名)@任意邮箱格式.后缀名` 格式的邮箱登录。密码字段可随意填写。  

## 补充信息  

您可以在[这里](./CommandList/commands_zh-CN.md)找到所有服务端指令的详细说明。<br>  
所有场景的列表[在此](./LevelsTable.md)。<br>  
所有敌人的列表[在此](./EnemiesTable.md)。<br>  

## Discord  

如果您希望参与讨论或协助该项目，请加入我们的 [Discord 服务器](https://discord.gg/gPvqhfdMU6)。  

## 注意  

该项目并非是为了取代官方服务器而创建。如需提交删除请求或要求说明，请通过 Telegram 联系我：@SuikoAkari
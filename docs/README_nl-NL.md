# EndField PS
[EN](../README.md) | [IT](./README_it-IT.md) | [RU](./README_ru-RU.md) | [CN](./README_zh-CN.md) | [NL](./README_nl-NL.md)

![Logo](https://socialify.git.ci/SuikoAkari/EndFieldPS/image?custom_description=Private+server+for+EndField&amp;description=1&amp;font=Jost&amp;forks=1&amp;issues=1&amp;language=1&amp;logo=https%3A%2F%2Farknights.wiki.gg%2Fimages%2F3%2F31%2FArknights_Endfield_logo.png&amp;name=1&amp;pattern=Circuit+Board&amp;pulls=1&amp;stargazers=1&amp;theme=Dark)

EndFieldPS is een privé server voor EndField CBT2.

## Huidige Functies

* Inloggen
* Avatar veranderen
* Team veranderen
* Scène veranderen (werkt voor sommige scènes)
* Data opslaan met MongoDB

## Installatie Hulp

1. Installeer:
   * [.NET SDK](https://dotnet.microsoft.com/en-us/download) (8.0.12 is aangeraden)
   * [MongoDB](https://www.mongodb.com/try/download/community)
   * [Fiddler Classic](https://www.telerik.com/fiddler/fiddler-classic) OF [mitmproxy](https://mitmproxy.org/)

    1. Als je *Fiddler Classic* installeert, zorg dat je "Decrypt HTTPS traffic" **aanzet** en het certificaat **installeert**!
    1. Je moet twee functies aanzetten via Tools (links boven in de menubard) -> Options -> HTTPS -> "Capture HTTPS CONNECTs" aanzetten en "Decrypt HTTPS traffic". Je kan ook het certificaat herinstalleren via Actions (rechts naast de "Capture HTTPS CONNECTs") -> Vertouw root certificaat, klik "Yes"
2. Download de [precompiled build](https://github.com/SuikoAkari/EndFieldPS/releases/latest) of compile het zelf
3. Doe de `Json` en `TableCfg` folders in de `EndFieldPS.exe` folder (Je kan [hier](https://github.com/PotRooms/EndFieldData/tree/main) een kopie downloaden)
4. Start de server (`EndFieldPS.exe`)
5. Overschrijf de `C:\Users\<YourUserName>\Documents\Fiddler2\Scripts\CustomRules.js` script (Of sla de originele op en maak een nieuwe met dezelfde naam) met het volgende script:
    * Je kan ook *Fiddler Classic* starten, ga naar `Rules -> Customize Rules` (CTRL + R) en sla het op, of selecteer de *FiddlerScript* tab

    ```javascript
    import System;
    import System.Windows.Forms;
    import Fiddler;
    import System.Text.RegularExpressions;

    class Handlers
    {
        static function OnBeforeRequest(oS: Session) {
            if(oS.host.Contains("gryphline.com") || oS.host.Contains("hg-cdn.com")) {
                if(oS.HTTPMethodIs("CONNECT")){
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

    Of je kan het mitmproxy commando gebruiken:

    ```shell
    mitmproxy -s ak.py
    ```

    ak.py:

    ```py
    import mitmproxy
    from mitmproxy import ctx, http
    class EndFieldModifier:
        def requestheaders(self,flow: mitmproxy.http.HTTPFlow):
            if "gryphline.com" in flow.request.host or "hg-cdn.com" in flow.request.host:
                if flow.request.method=="CONNECT":
                    return
                
                flow.request.scheme="http"
                flow.request.cookies.update({
                    "OriginalHost":flow.request.host,
                    "OriginalUrl":flow.request.url
                })
                flow.request.host="localhost"
                flow.request.port=5000
                ctx.log.info("URL:"+flow.request.url)
                
                
                
    addons=[
        EndFieldModifier()
    ]
    ```

6. Start *Fiddler Classic* - het zou moeten starten met de nieuwe *Custom Rules script* (Je kan het zien in het *FiddlerScript* tab)
7. Start de Game Client en begin met spelen! (Opmerking: Voor nu werkt alleen de OS client.)
8. Je moet een account maken doormiddel van `account create (username)` in de server console te typen, en dan ingame in te loggen met de mail `(gebruikersnaam)@watjewil.doemaarwat`. Er is geen wachtwoord, je kan dus gewoon onzin daar invullen.

## Extra Informatie

Je kan een beschrijving van alle commando's [hier](./CommandList/commands_en-US.md) vinden.<br>
De lijst van alle scènes zijn [hier](./LevelsTable.md).<br>
De lijst van alle enemies zijn [hier](./EnemiesTable.md).

## Discord

Als je wilt discussieren, of mee helpen, join dan onze [Discord Server](https://discord.gg/gPvqhfdMU6)!

## Opmerking

Dit project was niet gemaakt met de intentie om de officiële server te vervangen. Voor verwijderings aanvragen, of verduidelijkingen, kan je contact met me opnemen. Telegram: @SuikoAkari.

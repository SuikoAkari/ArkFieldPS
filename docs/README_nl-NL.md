# EndField PS
[EN](README.md) | [IT](docs/README_it-IT.md) | [RU](docs/README_ru-RU.md) | [NL](docs/README_nl-NL.md)

![Logo](https://socialify.git.ci/SuikoAkari/EndFieldPS/image?custom_description=Private+server+for+EndField&amp;description=1&amp;font=Jost&amp;forks=1&amp;issues=1&amp;language=1&amp;logo=https%3A%2F%2Farknights.wiki.gg%2Fimages%2F3%2F31%2FArknights_Endfield_logo.png&amp;name=1&amp;pattern=Circuit+Board&amp;pulls=1&amp;stargazers=1&amp;theme=Dark)

EndFieldPS is een privé server voor EndField CBT2.

## Huidige functies

* Inloggen
* Avatar veranderen
* Team veranderen
* Scène veranderen (werkt voor sommige scènes)
* Data opslaan met MongoDB

## Installatie hulp

1. Installeer [.NET SDK](https://dotnet.microsoft.com/en-us/download) (8.0.12 is aangeraden), [MongoDB](https://www.mongodb.com/try/download/community) en [Fiddler Classic](https://www.telerik.com/fiddler/fiddler-classic) OF [mitmproxy](https://mitmproxy.org/) als je het niet al hebt.
   * Als je *Fiddler Classic* installeert, zorg dat je "Decrypt HTTPS traffic" **aanzet** en het certificaat **installeert**!
   * Je moet twee functies via tools aanzetten (links boven in menubar) -> Options -> HTTPS -> zet "Capture HTTPS CONNECTs" aan en "Decrypt HTTPS traffic" aan. Je kan ook het certificaat herinstalleren via Actions (rechts naast "Capture HTTPS CONNECTs") -> Trust Root Certificate en klik "Yes"
2. Download de [precompiled build](https://github.com/SuikoAkari/EndFieldPS/releases/latest) of compile het zelf.
3. Doe de `Json` en `TableCfg` folders in de `EndFieldPS.exe` folder (die kan jedownloaden [hier](https://github.com/PotRooms/EndFieldData/tree/main) downloaden)
4. Start de server aan (`EndFieldPS.exe`)
5. Overschrijf de `C:\Users\<YourUserName>\Documents\Fiddler2\Scripts\CustomRules.js` script (of sla de originele op en maak een nieuwe met dezelfde naam) met het volgende script:
    * Je kan ook *Fiddler Classic* aanzetten, naar `Rules -> Customize Rules` (CTRL + R) gaan en het opslaan, of de *FiddlerScript* tab

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

6. Start *Fiddler Classic* - het zou moeten starten met het *Custom Rules script* (je kan het controleren in de *FiddlerScript* tab)
7. Start de Game Client en begin te spelen! (Opmerking: Alleen de OS client werkt voor nu.)
8. Je kan een account maken door `account create (username)` in de server console te typen, dan ingame inloggen met een mail zoals `(username)@randomemailformathere.whatyouwant`. Er is geen wachtwoord, dus je kan daar gewoon onzin typen.

## Discord

Als je iets wil discussieren, of meehelpen, join dan onze [Discord Server](https://discord.gg/gPvqhfdMU6)!

## Note

Dit project is niet gemaakt met de intentie om de officiële servers te vervangen. Voor verwijderingen of verduidelijkingen, neemt contact met mij op, Telegram: @SuikoAkari.

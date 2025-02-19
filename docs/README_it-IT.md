# ArkField PS
[EN](../README.md) | [IT](./README_it-IT.md) | [RU](./README_ru-RU.md) | [CN](./README_zh-CN.md) | [NL](./README_nl-NL.md)

![Logo](https://socialify.git.ci/SuikoAkari/ArkFieldPS/image?custom_description=Private+server+for+EndField&amp;description=1&amp;font=Jost&amp;forks=1&amp;issues=1&amp;language=1&amp;logo=https%3A%2F%2Farknights.wiki.gg%2Fimages%2F3%2F31%2FArknights_Endfield_logo.png&amp;name=1&amp;pattern=Circuit+Board&amp;pulls=1&amp;stargazers=1&amp;theme=Dark)

ArkFieldPS è un server privato per la CBT2 di EndField.

## Funzionalità attuali

* Login
* Switch personaggi
* Switch dei team
* Switch delle scene (solo alcune scene per ora)
* Salvare dati con MongoDB

## Passaggi di Installazione

1. Installa [.NET SDK](https://dotnet.microsoft.com/en-us/download) (8.0.12 è raccomandato), [MongoDB](https://www.mongodb.com/try/download/community) e [Fiddler Classic](https://www.telerik.com/fiddler/fiddler-classic) OPPURE [mitmproxy](https://mitmproxy.org/) se non li hai già
   * Mentre installi *Fiddler Classic*, assicurati di **abilitare** "Decrypt HTTPS traffic" e di **installare** il certificato!
   * Devi abilitare due funzionalità su Tools (nella barra menu in alto a sinistra) -> Options -> HTTPS -> Attiva "Capture HTTPS CONNECTs"e "Decrypt HTTPS traffic". Puoi anche re-installare il certificato da Actions (accanto a "Capture HTTPS CONNECTs") -> Trust Root Certificate e premi "Yes"
2. Scarica la [build precompilata](https://github.com/SuikoAkari/ArkFieldPS/releases/latest) o fai la build tu stesso
3. Metti le cartelle `Json` e `TableCfg` dentro la cartella di `ArkFieldPS.exe` (Puoi scaricarne una copia [qui](https://github.com/PotRooms/EndFieldData/tree/main))
4. Avvia il server (`ArkFieldPS.exe`)
5. Sovrascrivi lo script `C:\Users\<YourUserName>\Documents\Fiddler2\Scripts\CustomRules.js` (o esegui il backup di quello predefinito e crea un nuovo file con lo stesso nome) con lo script sottostante:
    * Puoi anche avviare *Fiddler Classic*, e andare su `Rules -> Customize Rules` (CTRL + R) e salvarlo li, oppure dalla scheda *FiddlerScript* 

    ```javascript
    import System;
    import System.Windows.Forms;
    import Fiddler;
    import System.Text.RegularExpressions;

    class Handlers
    {
        static function OnBeforeRequest(oS: Session) {
            if(
                oS.fullUrl.Contains("discord") ||
                oS.fullUrl.Contains("steam") ||
                oS.fullUrl.Contains("git") ||
                oS.fullUrl.Contains("yandex")
            //you can add any addresses if some sites don't work
            ) {
                oS.Ignore();
            }
        
            if (!oS.oRequest.headers.HTTPMethod.Equals("CONNECT")) {
                if(oS.fullUrl.Contains("gryphline.com") || oS.fullUrl.Contains("hg-cdn.com")) {
                    oS.fullUrl = oS.fullUrl.Replace("https://", "http://");
                    oS.host = "localhost"; // place another ip if you need
                    oS.port = 5000; //and port
                }
            }
        }
    };
    ```
    By Xannix
    Oppure puoi usare il comando mitmproxy:

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

6. Avvia *Fiddler Classic* - dovrebbe avviarsi con il nuovo *Custom Rules script* (puoi controllarlo nella scheda *FiddlerScript*)
7. Avvia il client del gioco e inizia a giocare! (Nota: Solo il client OS è supportato al momento)
8. Devi creare un account usando `account create (username)` nella console del server, e accedere al gioco utilizzando un email con il seguente formato `(username)@randomemailformathere.whatyouwant`. Non esiste una password, quindi puoi inserire una password casuale per il suo campo.

## Installation on Linux

You can read the manual on different Linux distros here:
|Linux distros     |Link  |
|:-----------------|:-----|
|Debian            |[Link](docs/RunOnLinuxServer.md#Debian-12)|
|ArchLinux         |[Link](docs/RunOnLinuxServer.md#ArchLinux)|
|Fedora            |[Link](docs/RunOnLinuxServer.md#fedora-workstation)|
|Ubuntu            |[Link](docs/RunOnLinuxServer.md#ubuntu)|

## Informazioni aggiuntive

Puoi trovare una descrizione di tutti i comandi del server [qui](./CommandList/commands_it-IT.md).<br>
[Qui](./LevelsTable.md) la lista di tutte le scene.<br>
[Qui](./EnemiesTable.md) la lista di tutti i nemici.
The list of all characters is [here](docs/CharactersTable.md).<br>

If you want to open the in-game console, go to `Settings -> Platform & Account -> Account Settings (Access Account button)`. To view avaiable commands, type `help`.

## Discord

Se vuoi discutere o aiutare con questo progetto, unisciti al nostro [Server Discord](https://discord.gg/gPvqhfdMU6)!

## Nota

Questo progetto non è stato creato con l'intenzione di sostituire i server ufficiali. Per richieste di rimozione o chiarimenti contattatemi su Telegram: @SuikoAkari.
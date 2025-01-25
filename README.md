
![Logo](https://socialify.git.ci/SuikoAkari/EndFieldPS/image?custom_description=Private+server+for+EndField&amp;description=1&amp;font=Jost&amp;forks=1&amp;issues=1&amp;language=1&amp;logo=https%3A%2F%2Farknights.wiki.gg%2Fimages%2F3%2F31%2FArknights_Endfield_logo.png&amp;name=1&amp;pattern=Circuit+Board&amp;pulls=1&amp;stargazers=1&amp;theme=Dark)

EndFieldPS is a private server for EndField CBT2

  
  
## Current Features

*   Login
*   Character switch
*   Team switch
*   Scene switch (work for some scene for now)
*   Save data with MongoDB

## Installation Steps:
1. Install [MongoDB](https://www.mongodb.com/try/download/community) and [Fiddler Classic](https://www.telerik.com/fiddler/fiddler-classic) if you don't have them already
2. Download the precompiled build or build it by yourself
3. Put `Json` and `TableCfg` folders inside the .exe folder (You can download a copy [here] (https://github.com/PotRooms/EndFieldData/tree/main))
4. Run the server
5. Overwrite the `C:\Users\<YourUserName>\Documents\Fiddler2\Scripts\CustomRules.js` script (or backup the default one and create a new file with the same name - you can also run Fiddler Classic, go to Rules -> Customize Rules (CTRL + R)) with the following script:
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
6. Run Fiddler Classic
7. Run the client and start to play! (Note: Only OS client is supported for now)
8. Need to create an account using: `account create (username)` in the server console, then login with email `(username)@randomemailformathere.whatyouwant`. There is no password so random password for the password field.
# Note
This project was not created with the intention of replacing the official servers. For removal requests or clarifications contact me on Telegram: @SuikoAkari
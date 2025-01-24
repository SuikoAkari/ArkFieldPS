
![Logo](https://socialify.git.ci/SuikoAkari/EndFieldPS/image?custom_description=Private+server+for+EndField&amp;description=1&amp;font=Jost&amp;forks=1&amp;issues=1&amp;language=1&amp;logo=https%3A%2F%2Farknights.wiki.gg%2Fimages%2F3%2F31%2FArknights_Endfield_logo.png&amp;name=1&amp;pattern=Circuit+Board&amp;pulls=1&amp;stargazers=1&amp;theme=Dark)

EndFieldPS is a private server for EndField CBT2

  
  
## Current Features

*   Login
*   Character switch
*   Team switch
*   Scene switch (work for some scene for now)
*   Save data with MongoDB

## Installation Steps:
1. Install mongodb if not have it already
2. Download precompiled build or build yourself
3. Put Json and TableCfg folder inside the .exe folder (You can download a copy here https://github.com/PotRooms/EndFieldData/tree/main)
4. Run the server
5. Run Fiddler classic and use this FiddlerScript
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
6. Run the client and start to play! (Note: Only OS client is supported for now)
7. Need to create an account using: account create (username) in the console, then login with email (username)@randomemailformathere.whatyouwant . There is no password so random password for the password field.
# Note
This project was not created with the intention of replacing the official servers. For removal requests or clarifications contact me on Telegram: @SuikoAkari